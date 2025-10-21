using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a user character, including its blueprint reference, assets, and equipped collectible tiers.
    /// Used in character-related API responses.
    /// </summary>
    public class Character
    {
        /// <summary>
        /// The unique identifier of the character.
        /// </summary>
        [JsonProperty("_id")]
        public string Id { get; set; }

        /// <summary>
        /// The identifier of the blueprint this character is based on.
        /// </summary>
        [JsonProperty("blueprintId")]
        public string BlueprintId { get; set; }

        /// <summary>
        /// The username associated with the character.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// The URL to the 3D model asset for the character.
        /// </summary>
        [JsonProperty("modelUrl")]
        public string ModelUrl { get; set; }

        /// <summary>
        /// The URL to the icon image for the character.
        /// </summary>
        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        /// <summary>
        /// A dictionary of asset types and their associated asset URLs.
        /// </summary>
        [JsonProperty("assets")]
        public IDictionary<string, string[]> Assets { get; set; }

        /// <summary>
        /// The date and time when the character was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the character was last updated, if available.
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The collectible tiers currently equipped by the character.
        /// </summary>
        [JsonProperty("equippedCollectibleTiers")]
        public CharacterCollectibleTiers EquippedCollectibleTiers { get; set; } = new CharacterCollectibleTiers();
    }

    /// <summary>
    /// Represents the number of collectibles equipped by a character, grouped by tier.
    /// Used as a property of <see cref="Character"/>.
    /// </summary>
    public class CharacterCollectibleTiers
    {
        /// <summary>
        /// The number of equipped secret-tier collectibles.
        /// </summary>
        [JsonProperty("secret")]
        public int Secret { get; set; }

        /// <summary>
        /// The number of equipped mythic-tier collectibles.
        /// </summary>
        [JsonProperty("mythic")]
        public int Mythic { get; set; }

        /// <summary>
        /// The number of equipped legendary-tier collectibles.
        /// </summary>
        [JsonProperty("legendary")]
        public int Legendary { get; set; }

        /// <summary>
        /// The number of equipped epic-tier collectibles.
        /// </summary>
        [JsonProperty("epic")]
        public int Epic { get; set; }

        /// <summary>
        /// The number of equipped rare-tier collectibles.
        /// </summary>
        [JsonProperty("rare")]
        public int Rare { get; set; }

        /// <summary>
        /// The number of equipped common-tier collectibles.
        /// </summary>
        [JsonProperty("common")]
        public int Common { get; set; }
    }
}