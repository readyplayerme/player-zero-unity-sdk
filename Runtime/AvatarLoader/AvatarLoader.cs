﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GLTFast;
using ReadyPlayerMe.Api.V1;
using ReadyPlayerMe.Data;
using UnityEngine;

namespace ReadyPlayerMe.AvatarLoader
{
    public class AvatarLoader
    {
        private readonly AvatarApi _avatarApi;
        private readonly MeshTransfer _meshTransfer;
        private readonly SkeletonBuilder _skeletonBuilder;

        public AvatarLoader()
        {
            _avatarApi = new AvatarApi();
            _meshTransfer = new MeshTransfer();
            _skeletonBuilder = new SkeletonBuilder();
        }

        public async Task<GameObject> PreviewAsync(string id, Dictionary<string, string> assets, GameObject template = null)
        {
            var previewUrl = _avatarApi.GenerateAvatarPreviewUrl(new AvatarPreviewRequest()
            {
                AvatarId = id,
                Params = new AvatarPreviewQueryParams()
                {
                    Assets = assets
                }
            });
                
            return await LoadAsync(id, template, previewUrl);
        }

        public async Task<GameObject> LoadAsync(string id, GameObject template = null, string loadFrom = null)
        {
            string baseModelId = null;
            
            if (string.IsNullOrEmpty(loadFrom))
            {
                var avatarResponse = await _avatarApi.FindAvatarByIdAsync(new AvatarFindByIdRequest()
                {
                    AvatarId = id,
                });

                loadFrom = avatarResponse.Data.GlbUrl;
                baseModelId = avatarResponse.Data.CreatedByApplicationId;
            }

            var gltf = new GltfImport();

            if (!await gltf.Load(loadFrom))
                return null;

            var avatar = new GameObject(id);

            await gltf.InstantiateSceneAsync(avatar.transform);

            if (template == null)
                return InitAvatar(avatar, id);

            // Update skeleton and transfer mesh
            var avatarSkeletonDefinition = Resources.Load<AvatarSkeletonDefinition>($"Character Avatar Bone Definitions/{baseModelId}");
            var animator = template.GetComponent<Animator>();
            if (animator.avatar == null)
            {
                _skeletonBuilder.Build(template, avatarSkeletonDefinition.GetHumanBones());
            }
            _meshTransfer.Transfer(avatar, template, avatarSkeletonDefinition);

            return InitAvatar(template, id);
        }

        protected virtual GameObject InitAvatar(GameObject avatar, string id)
        {
            var avatarData = avatar.GetComponent<AvatarData>();

            if (avatarData == null)
                avatarData = avatar.AddComponent<AvatarData>();

            return avatarData.Initialize(id);
        }
    }
}