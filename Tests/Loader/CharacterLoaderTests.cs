using NUnit.Framework;
using System.Threading;
using UnityEngine.TestTools;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ReadyPlayerMe.Tests.Loader
{
    public class CharacterLoaderTests
    {
        [Test, RequiresPlayMode]
        public async Task Load_Character_With_Valid_Glb_Url()
        {
            var characterManager = new CharacterManager();
            var character = await characterManager.LoadCharacter(TestConstants.CharacterId, TestConstants.TemplateId);
            
            Assert.IsNotNull(character);
        }
        
        [Test, RequiresPlayMode]
        public async Task Loaded_Character_Has_CharacterData_Component()
        {
            var characterManager = new CharacterManager();
            var character = await characterManager.LoadCharacter(TestConstants.CharacterId, TestConstants.TemplateId);

            Assert.IsNotNull(character);
            Assert.AreEqual(TestConstants.CharacterId, character.Id);
        }
        
        [Test, RequiresPlayMode]
        public async Task Cancel_Create()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(100);
            
            var characterManager = new CharacterLoader();
            var character = await characterManager.LoadAsync(TestConstants.CharacterId, null, cts.Token);
            
            cts.Dispose();
            Assert.IsNull(character);
        }
        
        [Test, RequiresPlayMode]
        public async Task Cancel_Preview()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(100);
            
            var characterManager = new CharacterLoader();
            var character = await characterManager.PreviewAsync(TestConstants.CharacterId, new Dictionary<string, string>(), TestConstants.TemplateId, cts.Token);
            
            cts.Dispose();
            Assert.IsNull(character);
        }
    }
}
