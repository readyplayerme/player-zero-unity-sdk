using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Provides API methods for retrieving and managing user characters.
    /// Inherits authentication functionality from <see cref="WebApiWithAuth"/>.
    /// </summary>
    public class CharacterApi : WebApiWithAuth
    {
        private const string Resource = "characters";

        /// <summary>
        /// Initializes a new instance of <see cref="CharacterApi"/> with API key authentication.
        /// </summary>
        public CharacterApi()
        {
            SetAuthenticationStrategy(new ApiKeyAuthStrategy());
        }

        /// <summary>
        /// Retrieves a character by its unique identifier.
        /// </summary>
        /// <param name="request">The request payload containing the character ID.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The API response containing the character data.</returns>
        public virtual async Task<CharacterFindByIdResponse> FindByIdAsync(CharacterFindByIdRequest request, CancellationToken cancellationToken = default)
        {
            return await Dispatch<CharacterFindByIdResponse>(
                new ApiRequest<string>()
                {
                    Url = $"{Settings.ApiBaseUrl}/v1/{Resource}/{request.Id}",
                    Method = "GET",
                    Headers = new Dictionary<string, string>()
                    {
                        { "Content-Type", "application/json" },
                    }
                }, cancellationToken
            );
        }
    }
}