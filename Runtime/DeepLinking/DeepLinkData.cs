using System;

namespace PlayerZero.Runtime.DeepLinking
{
    /// <summary>
    /// Represents data extracted from a deep link, including avatar and user identifiers.
    /// </summary>
    [Serializable]
    public struct DeepLinkData
    {
        /// <summary>
        /// The unique identifier for the avatar.
        /// </summary>
        public string AvatarId { get; set; }

        /// <summary>
        /// The username associated with the deep link.
        /// </summary>
        public string UserName { get; set; }
    }
}