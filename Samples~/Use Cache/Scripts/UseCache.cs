using UnityEngine;
using UnityEngine.UI;
using ReadyPlayerMe.Data;
using ReadyPlayerMe.Api.V1;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ReadyPlayerMe.Samples.UseCache
{
    public class UseCache : MonoBehaviour
    {
        [SerializeField] private CategoryController categoryController;
        [SerializeField] private AssetPageController assetPageController;
        [Space]
        [SerializeField] private Toggle categoriesToggle;
        [SerializeField] private Toggle assetsToggle;
        [SerializeField] private Toggle charactersToggle;
        [SerializeField] private GameObject mainUI;
        [SerializeField] private Transform characterPosition;
        
        private string selectedCategory;

        private AssetLoader assetLoader;
        private CharacterApi characterApi;
        private CharacterManager characterManager;
        
        private string characterId;
        private string baseModelId;
        private CharacterData characterData;

        private void Start()
        {
            EventAggregator.Instance.OnCategorySelected += OnCategorySelected;
            EventAggregator.Instance.OnAssetSelected += OnAssetSelected;
            EventAggregator.Instance.OnPageChanged += OnPageChanged;

            assetLoader = new AssetLoader();
            characterApi = new CharacterApi();
            characterManager = new CharacterManager();
        }

        public void DisplayUI()
        {
            if(mainUI.activeSelf) return;
            
            mainUI.SetActive(true);
            categoryController.LoadCategories(categoriesToggle.isOn);
            
            LoadCharacter();
        }

        private void OnCategorySelected(string category)
        {
            selectedCategory = category;
            assetPageController.LoadAssets(category, assetsToggle.isOn);
        }
        
        private void OnAssetSelected(Asset asset)
        {
            LoadAsset(asset);
        }
        
        private void OnPageChanged(int page)
        {
            assetPageController.LoadAssets(selectedCategory, assetsToggle.isOn, page);
        }
        
        private async void LoadCharacter()
        {
            baseModelId = await GetFirstBasemodelId();
            
            var createResponse = await characterApi.CreateAsync(new CharacterCreateRequest()
            {
                Payload = new CharacterCreateRequestBody()
                {
                    Assets = new Dictionary<string, string>
                    {
                        { "baseModel", baseModelId }
                    }
                }
            });
            
            characterId = createResponse.Data.Id;
            characterData = await characterManager.LoadCharacter(characterId, baseModelId);
            characterData.transform.SetParent(characterPosition, false);
        }
        
        private async void LoadAsset(Asset asset)
        {
            if(asset.Type == "baseModel")
            {
                await characterApi.UpdateAsync(new CharacterUpdateRequest()
                {
                    Id = characterId,
                    Payload = new CharacterUpdateRequestBody()
                    {
                        Assets = new Dictionary<string, string>
                        {
                            { "baseModel", asset.Id }
                        }
                    }
                });
                characterData = await characterManager.LoadCharacter(characterId, asset.Id);
                characterData.gameObject.transform.SetParent(characterPosition, false);
            }
            else
            {
                await characterApi.UpdateAsync(new CharacterUpdateRequest()
                {
                    Id = characterId,
                    Payload = new CharacterUpdateRequestBody()
                    {
                        Assets = new Dictionary<string, string>
                        {
                            { asset.Type, asset.Id }
                        }
                    }
                });
            
                characterData = await characterManager.LoadCharacter(characterId);
            }
            
            // characterData = await characterLoader.PreviewAsync(characterData, asset, charactersToggle.isOn);
        }
        
        private async Task<string> GetFirstBasemodelId()
        {
            AssetListResponse baseModelResponse = await assetLoader.ListAssetsAsync(new AssetListRequest
            {
                Params = new AssetListQueryParams()
                {
                    Type = "baseModel",
                }
            });

            return baseModelResponse.Data[0].Id;
        }
    }
}