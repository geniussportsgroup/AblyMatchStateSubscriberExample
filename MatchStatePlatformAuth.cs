using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeniusSports.AblyMatchStateSubscriberExample
{
    class MatchStatePlatformAuth
    {
        private readonly Options options;
        private readonly UrlProvider urlProvider;

        public MatchStatePlatformAuth(Options options, UrlProvider urlProvider)
        {
            this.options = options;
            this.urlProvider = urlProvider;
        }

        /// <summary>
        /// Gets access token that allows to access Match State Platform API.
        /// </summary>
        public async Task<string> GetAccessToken()
        {
            var body = new Dictionary<string, string> {
                {"grant_type", "client_credentials"},
                {"client_id", options.ClientId},
                {"client_secret", options.ClientSecret}
            };

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(urlProvider.AuthUrl, new FormUrlEncodedContent(body));
            var responseBody = await response.Content.ReadAsStringAsync();

            var tokenEnvelope = JsonConvert.DeserializeObject<JObject>(responseBody);
            var accessToken = tokenEnvelope["access_token"].ToString();
            return accessToken;
        }
    }
}
