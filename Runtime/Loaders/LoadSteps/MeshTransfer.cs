using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayerZero.Data;

//#if PZERO_GLTFAST
using GLTFast;
// #elif PZERO_UNITY_GLTF
         // using UnityGLTF;
         // #endif

namespace PlayerZero
{
    public class MeshTransfer
    {
        /// <summary>
        ///     Transfer meshes from source to target GameObject.
        ///     Destroys the source after transfer.
        /// </summary>
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
        /// Removes all non-attached SkinnedMeshRenderers from the target armature.
        /// </summary>
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
        /// Transfers all SkinnedMeshRenderers from sourceArmature to targetArmature,
        /// remapping bones by name using a complete bone map built from all target meshes.
        /// </summary>
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

        private Renderer[] GetRenderers(Transform armature)
        {
            List<Renderer> renderers = new List<Renderer>();
            GetRenderersRecursive(armature, renderers);
            return renderers.ToArray();
        }

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

        private Transform GetDefaultRootBone(Transform targetArmature)
        {
            var smr = targetArmature.GetComponentInChildren<SkinnedMeshRenderer>();
            return smr != null ? smr.rootBone : targetArmature;
        }
    }
}
