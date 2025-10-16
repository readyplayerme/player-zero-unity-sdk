using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayerZero.Data;

#if PZERO_UNITY_GLTF
using UnityGLTF;
#endif

namespace PlayerZero
{
    /// <summary>
    /// Provides functionality to transfer skinned meshes and bone mappings between GameObjects,
    /// including safe destruction of source objects and optional GLTF data transfer.
    /// </summary>
    public class MeshTransfer
    {
        /// <summary>
        /// Transfers all skinned meshes from the source GameObject to the target GameObject,
        /// remapping bones and updating the animator. The source GameObject is destroyed after transfer.
        /// </summary>
        /// <param name="source">The GameObject to transfer meshes from.</param>
        /// <param name="target">The GameObject to transfer meshes to.</param>
        public void Transfer(GameObject source, GameObject target)
        {
            var animator = target.GetComponent<Animator>();
            if (animator != null) animator.enabled = false;

            RemoveMeshes(target.transform);
            TransferMeshes(target.transform, source.transform, GetDefaultRootBone(target.transform));

            SafeDestroy(source);
            
            if (animator != null)
            {
                animator.enabled = true;
                animator.Rebind();
                animator.Update(0f);
            }
        }
        
        /// <summary>
        /// Safely destroys a GameObject, using immediate destruction in the editor and delayed destruction at runtime.
        /// </summary>
        /// <param name="obj">The GameObject to destroy.</param>
        private void SafeDestroy(GameObject obj)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Object.DestroyImmediate(obj);
                return;
            }
#endif
            Object.Destroy(obj);
        }

        /// <summary>
        /// Removes all SkinnedMeshRenderer GameObjects from the target armature that are not attached via TemplateAttachment.
        /// </summary>
        /// <param name="targetArmature">The target armature Transform.</param>
        private void RemoveMeshes(Transform targetArmature)
        {
            Renderer[] renderers = GetRenderers(targetArmature);
            foreach (Renderer renderer in renderers)
            {
                if (!renderer.gameObject.TryGetComponent<TemplateAttachment>(out _))
                {
                    SafeDestroy(renderer.gameObject);
                }
            }
        }

        /// <summary>
        /// Transfers all SkinnedMeshRenderers from the source armature to the target armature,
        /// remapping bones by name and updating mesh bounds. Also transfers GLTF data if available.
        /// </summary>
        /// <param name="targetArmature">The target armature Transform.</param>
        /// <param name="sourceArmature">The source armature Transform.</param>
        /// <param name="rootBone">The root bone Transform for the transferred meshes.</param>
        public void TransferMeshes(Transform targetArmature, Transform sourceArmature, Transform rootBone)
        {
            var targetBoneMap = GetAllTargetBonesMap(targetArmature);
            var sourceRenderers = GetRenderers(sourceArmature);

            var targetMeshRenderers = targetArmature.GetComponentsInChildren<SkinnedMeshRenderer>();
            Transform targetMeshParent = targetMeshRenderers.Length > 1
                ? targetMeshRenderers[1].transform.parent
                : targetMeshRenderers.FirstOrDefault()?.transform.parent ?? targetArmature;
            
#if PZERO_UNITY_GLTF
            var gltfcomponent = sourceArmature.GetComponentInChildren<InstantiatedGLTFObject>();
            if(gltfcomponent != null)
            {
                gltfcomponent.TransferComponent(targetArmature.gameObject);
            }
#endif

            foreach (Renderer renderer in sourceRenderers)
            {
                renderer.transform.SetParent(targetMeshParent, worldPositionStays: false);
                renderer.gameObject.SetActive(true);

                if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
                {
                    Transform[] originalBones = skinnedMeshRenderer.bones;
                    Transform[] remappedBones = new Transform[originalBones.Length];

                    for (int i = 0; i < originalBones.Length; i++)
                    {
                        string boneName = originalBones[i]?.name;
                        if (boneName != null && targetBoneMap.TryGetValue(boneName, out var targetBone))
                        {
                            remappedBones[i] = targetBone;
                        }
                    }

                    skinnedMeshRenderer.rootBone = rootBone;
                    skinnedMeshRenderer.bones = remappedBones;

                    if (skinnedMeshRenderer.sharedMesh != null && skinnedMeshRenderer.sharedMesh.isReadable)
                    {
                        skinnedMeshRenderer.sharedMesh.RecalculateBounds();
                    }
                }
            }

            if (rootBone != null)
                rootBone.SetAsLastSibling();
        }

        /// <summary>
        /// Builds a dictionary mapping bone names to Transforms from all SkinnedMeshRenderers in the target armature.
        /// </summary>
        /// <param name="targetArmature">The target armature Transform.</param>
        /// <returns>A dictionary mapping bone names to Transforms.</returns>
        private Dictionary<string, Transform> GetAllTargetBonesMap(Transform targetArmature)
        {
            var skinnedMeshes = targetArmature.GetComponentsInChildren<SkinnedMeshRenderer>();
            var boneMap = new Dictionary<string, Transform>();

            foreach (var smr in skinnedMeshes)
            {
                foreach (var bone in smr.bones)
                {
                    if (bone != null && !boneMap.ContainsKey(bone.name))
                    {
                        boneMap[bone.name] = bone;
                    }
                }
            }

            return boneMap;
        }

        /// <summary>
        /// Collects all SkinnedMeshRenderer components from the given armature, excluding those attached via TemplateAttachment.
        /// </summary>
        /// <param name="armature">The armature Transform to search.</param>
        /// <returns>An array of Renderer components.</returns>
        private Renderer[] GetRenderers(Transform armature)
        {
            List<Renderer> renderers = new List<Renderer>();
            GetRenderersRecursive(armature, renderers);
            return renderers.ToArray();
        }

        /// <summary>
        /// Recursively collects SkinnedMeshRenderer components from the hierarchy, skipping TemplateAttachment nodes.
        /// </summary>
        /// <param name="parent">The parent Transform to search.</param>
        /// <param name="renderers">The list to populate with found Renderer components.</param>
        private void GetRenderersRecursive(Transform parent, List<Renderer> renderers)
        {
            if (parent.GetComponent<TemplateAttachment>() != null)
                return;

            foreach (Transform child in parent)
            {
                if (child.TryGetComponent<SkinnedMeshRenderer>(out var smr))
                {
                    renderers.Add(smr);
                }
                GetRenderersRecursive(child, renderers);
            }
        }

        /// <summary>
        /// Gets the default root bone from the first SkinnedMeshRenderer in the target armature,
        /// or returns the armature itself if none is found.
        /// </summary>
        /// <param name="targetArmature">The target armature Transform.</param>
        /// <returns>The root bone Transform.</returns>
        private Transform GetDefaultRootBone(Transform targetArmature)
        {
            var smr = targetArmature.GetComponentInChildren<SkinnedMeshRenderer>();
            return smr != null ? smr.rootBone : targetArmature;
        }
    }
}
