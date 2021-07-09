using CommandLine;

namespace GeniusSports.AblyMatchStateSubscriberExample
{
    class Options
    {
        [Option("client-id", Required = true, HelpText = "Get from  Genius Sports support team")]
        public string ClientId { get; }

        [Option("client-secret", Required = true, HelpText = "Get from  Genius Sports support team")]
        public string ClientSecret { get; }

        [Option("api-key", Required = true, HelpText = "Get from  Genius Sports support team")]
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