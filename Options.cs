using CommandLine;

namespace GeniusSports.AblyMatchStateSubscriberExample
{
    class Options
    {
        private const string SecretHelpText =
            "Get from Genius Sports support team. Email: apikey@geniussports.com";

        [Option("client-id", Required = true, HelpText = SecretHelpText)]
        public string ClientId { get; }

        [Option("client-secret", Required = true, HelpText = SecretHelpText)]
        public string ClientSecret { get; }

        [Option("api-key", Required = true, HelpText = SecretHelpText)]
        public string ApiKey { get; }

        [Option("environment", Required = true, HelpText = "Possible values: ci, uat, prod")]
        public Environment Environment { get; }

        public Options(string clientId, string clientSecret, string apiKey, Environment environment)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            ApiKey = apiKey;
            Environment = environment;
        }
    }
}