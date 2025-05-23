#if PZERO_GLTFAST
using GLTFast;
#elif PZERO_UNITY_GLTF
using UnityGLTF;
#endif
using System.Linq;
using UnityEngine;
using PlayerZero.Data;
using PlayerZero.Api.V1;
using System.Threading.Tasks;
using PlayerZero.Api;
using PlayerZero.Runtime.Sdk;
using Object = UnityEngine.Object;

namespace PlayerZero
{
    public class CharacterLoader 
    {
        private readonly CharacterApi _characterApi;
        private readonly MeshTransfer _meshTransfer;
        private CharacterTemplateConfig templateConfig;
        private string applicationId;
        
        /// <summary>
        ///     Initializes a new instance of the CharacterLoader class.
        /// </summary>
        public CharacterLoader(CharacterTemplateConfig templateConfig = null)
        {
            _characterApi = new CharacterApi();
            _meshTransfer = new MeshTransfer();
            this.templateConfig = templateConfig;
        }
        
        /// <summary>
        ///     Loads a character based on the given character ID onto the given blueprint.
        ///     Used for multiplayer games where the character is required to be prepared ahead of time.
        /// </summary>
        /// <param name="characterId">The character ID of the character to load. </param>
        /// <param name="template">The template game object to load the character on to. </param>
        /// <param name="meshParent">The parent object to attach the character mesh to. </param>
        /// <param name="config">The configuration to use when loading the character. </param>
        /// <param name="blueprintId">The target blueprint for this character. </param>
        /// <returns> A CharacterData object representing the loaded character. </returns>
        public async Task<CharacterData> LoadAsync(
            string characterId,
            GameObject template,
            GameObject meshParent = null,
            CharacterLoaderConfig config = null,
            string blueprintId = null
            )
        {
            var response = await _characterApi.FindByIdAsync(new CharacterFindByIdRequest()
            {
                Id = characterId,
            });
            
            if (blueprintId == null)
            {
                blueprintId = response.Data.BlueprintId;
            }
            
            var characterData = template.AddComponent<CharacterData>();
            characterData.Initialize(response.Data.Id, blueprintId);

            return await SetupCharacter(characterData, config, meshParent, response.Data.ModelUrl, blueprintId, response.Data.Id);
        }

        /// <summary>
        ///     Loads a character based on the given character ID.
        /// </summary>
        /// <param name="characterId">The character ID of the character to load. </param>
        /// <param name="templateTag">The tag of the template to load. </param>
        /// <param name="config">The configuration to use when loading the character. </param>
        /// <param name="blueprintId">The target blueprint for this character. </param>
        /// <returns> A CharacterData object representing the loaded character. </returns>
        public async Task<CharacterData> LoadAsync(
            string characterId,
            string templateTag = "",
            CharacterLoaderConfig config = null,
            string blueprintId = null
            )
        {
            var response = await _characterApi.FindByIdAsync(new CharacterFindByIdRequest()
            {
                Id = characterId,
            });
            
            if (blueprintId == null)
            {
                blueprintId = response.Data.BlueprintId;
            }
            
            var templatePrefab = GetTemplate(blueprintId, templateTag);
            
            if (templatePrefab == null)
            {
                Debug.LogError( $"Failed to load character template for character with ID {characterId}." );
                return null;
            }
            var templateInstance = Object.Instantiate(templatePrefab);
            var characterData = templateInstance.AddComponent<CharacterData>();
            characterData.Initialize(response.Data.Id, blueprintId);
            
            return await SetupCharacter(characterData, config, null, response.Data.ModelUrl, blueprintId, response.Data.Id);
        }

        private async Task<CharacterData> SetupCharacter(CharacterData characterData, CharacterLoaderConfig config, GameObject meshParent, string modelUrl, string blueprintId, string characterId)
        {
            if(config == null)
            {
                config = new CharacterLoaderConfig();
            }
            var query= QueryBuilder.BuildQueryString(config);
            var url = $"{modelUrl}?{query}&targetBlueprintId={blueprintId}";

            var playerZeroCharacter = await GltfLoader.LoadModelAsync(url);
            
            if (playerZeroCharacter == null)
            {
                Debug.LogError( $"Failed to load character model for character with ID {characterId}." );
                return null;
            }
            var characterObject = new GameObject(characterId);
            playerZeroCharacter.transform.parent = characterObject.transform;
            playerZeroCharacter.transform.localPosition = Vector3.zero;
            playerZeroCharacter.transform.localEulerAngles = Vector3.zero;
            playerZeroCharacter.transform.localScale = Vector3.one;
            
            var animator = characterData.gameObject.GetComponent<Animator>();
            if( animator == null )
            {
                animator = characterData.gameObject.AddComponent<Animator>();
            }
            animator.enabled = false;
            
            _meshTransfer.Transfer(characterObject, meshParent ?? characterData.gameObject);
            characterData.gameObject.SetActive(true);
            
            animator.enabled = true;
        
            return characterData;
        }
        
        /// <summary>
        ///     Retrieves a template based on the given template tag or ID.
        /// </summary>
        /// <param name="templateTagOrId"> The template tag or ID of the character to load. </param>
        /// <returns> A GameObject representing the template. </returns>
        protected virtual GameObject GetTemplate(string blueprintId, string tag = "")
        {
            if (string.IsNullOrEmpty(blueprintId))
                return null;

            if (templateConfig == null) // load default if not set
            {
                if (string.IsNullOrEmpty(applicationId))
                {
                    applicationId = Resources.Load<Settings>( "PlayerZeroSettings")?.ApplicationId;
                }
                templateConfig = Resources.Load<CharacterTemplateConfig>(applicationId);
            }
            if (templateConfig == null)
            {
                Debug.LogError("Character template config not found.");
                return null;
            }
            var blueprintTemplate = templateConfig.Templates.ToList().FirstOrDefault(p => p.BlueprintId == blueprintId) ?? templateConfig.Templates[0];
            return blueprintTemplate.GetPrefabByTag(tag);
        }
    }
}