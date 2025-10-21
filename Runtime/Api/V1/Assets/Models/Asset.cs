using System;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents an asset returned by the API, including metadata such as ID, name, type, and URLs for the GLB model and icon.
    /// Used for deserializing asset details from API responses.
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// The unique identifier of the asset.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The display name of the asset.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL to the GLB model file for the asset.
        /// </summary>
        public string GlbUrl { get; set; }

        /// <summary>
        /// The URL to the icon image for the asset.
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// The type/category of the asset.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The date and time when the asset was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the asset was last updated, if available.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}