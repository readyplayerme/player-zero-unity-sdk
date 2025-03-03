using System;

namespace PlayerZero.Runtime.DeepLinking
{
    [Serializable]
    public struct DeepLinkData
    {
        public string AvatarId { get; set; }
        public string UserName { get; set; }
    }
}