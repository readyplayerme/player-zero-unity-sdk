using System;
using UnityEngine;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReadyPlayerMe.Runtime.Api.V1.Avatars;
using ReadyPlayerMe.Runtime.Api.V1.Avatars.Models;
using UnityEngine.TestTools;

namespace ReadyPlayerMe.Tests.Api.V1
{
    public class AvatarApiTests
    {
        private readonly AvatarApi characterApi = new AvatarApi();
        
        [Test, Order(0)]
        public async Task Create_Character()
        {
            // Arrange
            var request = new AvatarCreateRequest()
            {
                Payload = new AvatarCreateRequestBody
                {
                    ApplicationId = "6628c280ecb07cb9d9cd7238"
                }
            };
            
            // Act
            var response = await characterApi.CreateAvatarAsync(request);
            Debug.Log(response.Data.GlbUrl);
            
            // Assert
            Assert.IsTrue(Uri.TryCreate(response.Data.GlbUrl, UriKind.Absolute, out _));
        }
        
        [Test, RequiresPlayMode]
        public async Task Update_Character()
        {
            // Arrange
            var request = new AvatarUpdateRequest()
            {
                AvatarId = "6628d1c497cb7a2453b807b1",
                Payload = new AvatarUpdateRequestBody()
                {
                    Assets = new Dictionary<string, string>
                    {
                        { "top", "662a22be282104d791d4a123" }
                    }
                }
            };
            
            // Act
            var response = await characterApi.UpdateAvatarAsync(request);
            Debug.Log(response.Data.GlbUrl);
            
            // Assert
            Assert.IsTrue(Uri.TryCreate(response.Data.GlbUrl, UriKind.Absolute, out _));
        }
        
        [Test, RequiresPlayMode]
        public async Task Preview_Character()
        {
            // Arrange
            var request = new AvatarPreviewRequest()
            {
                AvatarId = "6628d1c497cb7a2453b807b1",
                Params = new AvatarPreviewQueryParams()
                {
                    Assets = new Dictionary<string, string>
                    {
                        {"top", "662a22be282104d791d4a123"}
                    }
                }
            };
            
            // Act
            var response = await characterApi.PreviewAvatarAsync(request);
            
            // Assert
            Assert.NotNull(response);
        }
    }
}
