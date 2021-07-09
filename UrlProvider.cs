using System;

namespace GeniusSports.AblyMatchStateSubscriberExample
{
    class UrlProvider
    {
        public string AuthUrl { get; }
        public string MatchStatePlatformBaseUrl { get; }

        public UrlProvider(Environment environment)
        {
            switch (environment)
            {
                case Environment.Ci:
                    AuthUrl = "https://ci.auth.api.geniussports.com/oauth2/token";
                    MatchStatePlatformBaseUrl = "https://platform.ci.matchstate.api.geniussports.com/api/v1";
                    break;
                case Environment.Uat:
                    AuthUrl = "https://uat.auth.api.geniussports.com/oauth2/token";
                    MatchStatePlatformBaseUrl = "https://platform.uat.matchstate.api.geniussports.com/api/v1";
                    break;
                case Environment.Prod:
                    AuthUrl = "https://auth.api.geniussports.com/oauth2/token";
                    MatchStatePlatformBaseUrl = "https://platform.matchstate.api.geniussports.com/api/v1";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(environment), environment, null);
            }
        }
    }
}