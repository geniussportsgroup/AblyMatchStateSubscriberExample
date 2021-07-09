using System;
using System.Threading.Tasks;
using IO.Ably;
using IO.Ably.Realtime;

namespace GeniusSports.AblyMatchStateSubscriberExample
{
    class MatchStateAbly
    {
        private readonly string sourceId;
        private readonly int sportId;
        private readonly int fixtureId;
        private readonly MatchStatePlatform matchStatePlatform;

        public MatchStateAbly(string sourceId, int sportId, int fixtureId, MatchStatePlatform matchStatePlatform)
        {
            this.sourceId = sourceId;
            this.sportId = sportId;
            this.fixtureId = fixtureId;
            this.matchStatePlatform = matchStatePlatform;
        }

        private async Task<object> AuthCallback(TokenParams arg)
        {
            (_, string accessToken) = await matchStatePlatform.GetLiveAccess(sourceId, sportId, fixtureId);
            return new TokenDetails
            {
                Token = accessToken
            };
        }

        public async Task Subscribe(Action<Message> messageReceived)
        {
            (string channelName, string accessToken) = 
                await matchStatePlatform.GetLiveAccess(sourceId, sportId, fixtureId);

            var ably = new AblyRealtime(new ClientOptions
            {
                Token = accessToken,
                AuthCallback = AuthCallback,
                Environment = "geniussports",
                FallbackHosts = new[]
                {
                    "geniussports-a-fallback.ably-realtime.com",
                    "geniussports-b-fallback.ably-realtime.com",
                    "geniussports-c-fallback.ably-realtime.com",
                    "geniussports-d-fallback.ably-realtime.com",
                    "geniussports-e-fallback.ably-realtime.com"
                }
            });

            IRealtimeChannel channel = ably.Channels.Get(channelName);
            channel.Subscribe(messageReceived);
        }
    }
}