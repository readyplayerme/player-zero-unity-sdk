﻿using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ReadyPlayerMe.Phoenix
{
    public class AuthAPIRequests
    {
        private readonly string domain;
        private readonly IDictionary<string, string> headers = CommonHeaders.GetHeadersWithAppId();
        private readonly string rpmAuthBaseUrl;

        private readonly WebRequestDispatcher webRequestDispatcher;

        public AuthAPIRequests(string domain)
        {
            this.domain = domain;
            webRequestDispatcher = new WebRequestDispatcher();

            rpmAuthBaseUrl = string.Format(Env.RPM_SUBDOMAIN_BASE_URL, domain);
        }

        public async Task<UserSession> LoginAsAnonymous()
        {
            var response = await webRequestDispatcher.SendRequest<Response>($"{rpmAuthBaseUrl}/users", HttpMethod.POST, headers);

            var data = AuthDataConverter.ParseResponse(response.Text);
            return JsonConvert.DeserializeObject<UserSession>(data!.ToString());
        }

        public async Task SendCodeToEmail(string email, string userId = "")
        {
            var data = new Dictionary<string, string>
            {
                { AuthConstants.EMAIL, email },
                { AuthConstants.AUTH_TYPE, AuthConstants.AUTH_TYPE_CODE }
            };

            if (!string.IsNullOrEmpty(userId))
            {
                data.Add(AuthConstants.USER_ID, userId);
            }

            var payload = AuthDataConverter.CreatePayload(data);

            var response = await webRequestDispatcher.SendRequest<Response>($"{rpmAuthBaseUrl}/auth/start", HttpMethod.POST, headers, payload);
        }

        public async Task<UserSession> LoginWithCode(string code, string userIdToMerge = null)
        {
            var body = new Dictionary<string, string>
            {
                { AuthConstants.AUTH_TYPE_CODE, code }
            };
            if (userIdToMerge != null)
            {
                body.Add(AuthConstants.USER_ID, userIdToMerge);
            }
            var payload = AuthDataConverter.CreatePayload(body);

            var response = await webRequestDispatcher.SendRequest<Response>($"{rpmAuthBaseUrl}/auth/login", HttpMethod.POST, headers, payload);

            var data = AuthDataConverter.ParseResponse(response.Text);
            return JsonConvert.DeserializeObject<UserSession>(data!.ToString());
        }

        public async Task Signup(string email, string userId)
        {
            var data = new Dictionary<string, string>
            {
                { AuthConstants.EMAIL, email },
                { AuthConstants.AUTH_TYPE, AuthConstants.AUTH_TYPE_PASSWORD },
                { AuthConstants.USER_ID, userId }
            };

            var payload = AuthDataConverter.CreatePayload(data);
            var response = await webRequestDispatcher.SendRequest<Response>($"{rpmAuthBaseUrl}/auth/start", HttpMethod.POST, headers, payload);
        }

        public async Task<(string, string)> RefreshToken(string token, string refreshToken)
        {
            var url = $"{rpmAuthBaseUrl}/auth/refresh";

            var payload = AuthDataConverter.CreatePayload(new Dictionary<string, string>
            {
                { AuthConstants.TOKEN, token },
                { AuthConstants.REFRESH_TOKEN, refreshToken }
            });

            var response = await webRequestDispatcher.SendRequest<Response>(url, HttpMethod.POST, headers, payload);

            var data = AuthDataConverter.ParseResponse(response.Text);
            var newToken = data[AuthConstants.TOKEN]!.ToString();
            var newRefreshToken = data[AuthConstants.REFRESH_TOKEN]!.ToString();
            return (newToken, newRefreshToken);
        }

    }
}
