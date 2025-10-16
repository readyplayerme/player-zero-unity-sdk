using System;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a character blueprint, including its metadata and associated character model.
    /// Used in blueprint listing and retrieval API responses.
    /// </summary>
    [Serializable]
    public class CharacterBlueprint
    {
        /// <summary>
        /// The display name of the character blueprint.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// The unique identifier of the character blueprint.
        /// </summary>
        [JsonProperty("id")]
        public string Id;

        /// <summary>
        /// The character model associated with this blueprint.
        /// </summary>
        [JsonProperty("characterModel")]
        public BlueprintCharacterModel CharacterModel { get; set; } = new BlueprintCharacterModel();

        /// <summary>
        /// The date and time when the blueprint was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the blueprint was last updated.
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// Represents the model details for a character blueprint, including asset URLs and metadata.
    /// Used as a property of <see cref="CharacterBlueprint"/>.
    /// </summary>
    [Serializable]
    public class BlueprintCharacterModel
    {
        /// <summary>
        /// The display name of the character model.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// The unique identifier of the character model.
        /// </summary>
        [JsonProperty("id")]
        public string Id;

        /// <summary>
        /// The URL to the 3D model asset for the character.
        /// </summary>
        [JsonProperty("modelUrl")]
        public string ModelUrl;

        /// <summary>
        /// The URL to the icon image for the character.
        /// </summary>
        [JsonProperty("iconUrl")]
        public string IconUrl;

        /// <summary>
        /// The date and time when the character model was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the character model was last updated.
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
