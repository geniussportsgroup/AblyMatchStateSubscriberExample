using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace GeniusSports.AblyMatchStateSubscriberExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var parser = new Parser(settings =>
            {
                settings.HelpWriter = Console.Error;
                settings.CaseInsensitiveEnumValues = true;
            });

            await parser.ParseArguments<Options>(args)
                .WithParsedAsync(async options =>
                {
                    Console.WriteLine($"Running on '{options.Environment.ToString().ToLower()}' environment");

                    var urlProvider = new UrlProvider(options.Environment);
                    var matchStatePlatformAuth = new MatchStatePlatformAuth(options, urlProvider);
                    var matchStatePlatform = new MatchStatePlatform(options, urlProvider, matchStatePlatformAuth);

                    const string sourceId = "GeniusPremium";
                    const int sportId = 17; // American Football

                    int? fixtureId = await GetFixtureId(sourceId, sportId, matchStatePlatform);
                    if (fixtureId == null)
                    {
                        Console.WriteLine($"No scheduled fixtures for source '{sourceId}' and sport '{sportId}'");
                        return;
                    }

                    Console.WriteLine($"Subscribing for match state for fixture '{fixtureId}'");
                    await SubscribeForMatchState(sourceId, sportId, fixtureId.Value, matchStatePlatform);

                    Console.ReadKey();
                });
        }

        private static async Task<int?> GetFixtureId(
            string sourceId, int sportId, MatchStatePlatform matchStatePlatform)
        {
            var from = DateTime.UtcNow.AddHours(-1);
            var to = DateTime.UtcNow.AddHours(3);

            IList<string> fixtureIds = await matchStatePlatform.GetScheduledFixtureIds(sourceId, sportId, from, to);

            return fixtureIds.Any() ? int.Parse(fixtureIds[0]) : (int?) null;
        }

        private static async Task SubscribeForMatchState(
            string sourceId, int sportId, int fixtureId, MatchStatePlatform matchStatePlatform)
        {
            var matchStateAbly = new MatchStateAbly(sourceId, sportId, fixtureId, matchStatePlatform);
            await matchStateAbly.Subscribe(
                message => Console.WriteLine($"{message.Name}:{message.Data}"));
        }
    }
}
