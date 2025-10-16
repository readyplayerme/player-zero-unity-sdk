namespace PlayerZero.Api.V3
{
    /// <summary>
    /// Represents a request payload containing an avatar code,
    /// typically used for avatar authentication or retrieval.
    /// </summary>
    public class AvatarCodeRequest
    {
        /// <summary>
        /// The code associated with the avatar.
        /// </summary>
        public string Code { get; set; }
    }
}
