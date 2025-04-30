using System.Threading;
using System.Threading.Tasks;

namespace PlayerZero.Api
{
    public interface IAuthenticationStrategy
    {
        Task AddAuthToRequestAsync<T>(ApiRequest<T> request, CancellationToken cancellationToken = default);

        Task<bool> TryRefreshAsync<T>(ApiRequest<T> request, CancellationToken cancellationToken = default);
    }
}