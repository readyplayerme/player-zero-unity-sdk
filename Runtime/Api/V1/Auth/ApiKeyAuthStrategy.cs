﻿using System.Threading;
using System.Threading.Tasks;
using PlayerZero.Data;
using UnityEngine;

namespace PlayerZero.Api.V1
{
    public class ApiKeyAuthStrategy : IAuthenticationStrategy
    {
        private readonly Settings _settings;

        public ApiKeyAuthStrategy()
        {
            _settings = Resources.Load<Settings>("PlayerZeroSettings");
        }
        
        public Task AddAuthToRequestAsync<T>(ApiRequest<T> request, CancellationToken cancellationToken = default)
        {
            request.Headers["X-API-KEY"] = _settings.ApiKey;

            return Task.CompletedTask;
        }

        public Task<bool> TryRefreshAsync<T>(ApiRequest<T> request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}