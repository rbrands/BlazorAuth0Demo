using Flurl.Http;
using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorAuth0Demo.Shared;
using Newtonsoft.Json;


namespace BlazorAuth0Demo.Server.Repositories
{
    public class Auth0Repository
    {
        private readonly Auth0Config _auth0Config;
        private string _accessToken;
        private DateTime _accessTokenValid = DateTime.UtcNow;

        public Auth0Repository(Auth0Config auth0Config)
        {
            _auth0Config = auth0Config;
        }
        /// <summary>
        /// Gets an access token for accessing the management API.
        /// </summary>
        /// <returns></returns>
        /// <seealso cref="https://auth0.com/docs/api/management/v2/get-access-tokens-for-production"/>
        public async Task<string> GetAccessToken()
        {
            if (String.IsNullOrEmpty(_accessToken) || _accessTokenValid.CompareTo(DateTime.UtcNow) > 1)
            {
                object request = new
                {
                    grant_type = "client_credentials",
                    client_id = _auth0Config.ClientId,
                    client_secret = _auth0Config.ClientSecret,
                    audience = $"https://{_auth0Config.Domain}/api/v2/"
                };

                dynamic response = await $"https://{_auth0Config.Domain}/oauth/token".PostUrlEncodedAsync(request).ReceiveJson();
                _accessToken = response.access_token;
                _accessTokenValid = DateTime.UtcNow.AddSeconds(response.expires_in);
            }
            return _accessToken;
        }

        /// <summary>
        /// Add the given permission for the referenced API to the given user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        /// <seealso cref="https://auth0.com/docs/api/management/guides/users/assign-permissions-users"/>
        public async Task AssignPermission(string apiIdentifier, string userId, string permission)
        {
            object request = new
            {
                permissions = new object[]
                {
                    new
                    {
                        resource_server_identifier = apiIdentifier,
                        permission_name = permission
                    }
                }
            };
            string accessToken = await GetAccessToken();
            await $"https://{_auth0Config.Domain}/api/v2/users/{userId}/permissions"
                                     .WithOAuthBearerToken(accessToken)
                                     .PostJsonAsync(request);
        }
        /// <summary>
        /// Updates the given metadata for referenced user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        /// <see cref="https://auth0.com/docs/users/guides/update-metadata-properties-with-management-api"/>
        public async Task UpdateUserMetadata(string userId, Auth0UserMetaData userData)
        {
            Auth0UserMetaDataRequest request = new Auth0UserMetaDataRequest(userData);
            string accessToken = await GetAccessToken();
            await $"https://{_auth0Config.Domain}/api/v2/users/{userId}"
                                     .WithOAuthBearerToken(accessToken)
                                     .WithHeader("content-type", "application/json")
                                     .PatchStringAsync(JsonConvert.SerializeObject(request));
        }
    }

    public class Auth0Config
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class Auth0UserMetaDataRequest
    {
        [JsonProperty(PropertyName = "user_metadata")]
        public Auth0UserMetaData UserData { get; set; }
        public Auth0UserMetaDataRequest()
        {
        }
        public Auth0UserMetaDataRequest(Auth0UserMetaData userData)
        {
            UserData = userData;
        }
    }
}
