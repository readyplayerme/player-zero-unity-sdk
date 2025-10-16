namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a request to find a character by its unique identifier.
    /// Used in character retrieval API endpoints.
    /// </summary>
    public class CharacterFindByIdRequest
    {
        /// <summary>
        /// The unique identifier of the character to retrieve.
        /// </summary>
        public string Id { get; set; }
    }

}