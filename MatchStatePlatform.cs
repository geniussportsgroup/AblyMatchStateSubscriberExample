using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeniusSports.AblyMatchStateSubscriberExample
{
    class MatchStatePlatform
    {
        private readonly Options options;
        private readonly UrlProvider urlProvider;
        private readonly MatchStatePlatformAuth auth;

        public MatchStatePlatform(Options options, UrlProvider urlProvider, MatchStatePlatformAuth auth)
        {
            this.options = options;
            this.urlProvider = urlProvider;
            this.auth = auth;
        }

        /// <summary>
        /// Gets scheduled fixtures between <paramref name="from"/> and <paramref name="to"/> for a given
        /// <paramref name="sourceId"/> and <paramref name="sportId"/>.
        /// </summary>
        public async Task<IList<string>> GetScheduledFixtureIds(string sourceId, int sportId, DateTime from, DateTime to)
        {
            var baseUrl = urlProvider.MatchStatePlatformBaseUrl;
            var url = $"{baseUrl}/sources/{sourceId}/sports/{sportId}/schedule?from={from:s}&to={to:s}";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", await auth.GetAccessToken());
            httpClient.DefaultRequestHeaders.Add("x-api-key", options.ApiKey);

            var response = await httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            var schedule = JsonConvert.DeserializeObject<JArray>(responseBody);
            return schedule != null 
                ? schedule.Select(x => x["fixtureId"].ToString()).ToList() 
                : new List<string>();
        }

        /// <summary>
        /// Gets Ably channel name and access token for given <paramref name="sourceId"/>, <paramref name="sportId"/>
        /// and <paramref name="fixtureId"/>.
        /// Channel name and access token are needed for subscribing for Ably match state feed.
        /// </summary>
        public async Task<(string channelName, string accessToken)> GetLiveAccess(string sourceId, int sportId, int fixtureId)
        {
            var baseUrl = urlProvider.MatchStatePlatformBaseUrl;
            var url = $"{baseUrl}/sources/{sourceId}/sports/{sportId}/fixtures/{fixtureId}/liveaccess";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", await auth.GetAccessToken());
            httpClient.DefaultRequestHeaders.Add("x-api-key", options.ApiKey);

            var response = await httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            var liveAccess = JsonConvert.DeserializeObject<JObject>(responseBody);
            return (liveAccess["channelName"].ToString(), liveAccess["accessToken"].ToString());
        }
    }
}