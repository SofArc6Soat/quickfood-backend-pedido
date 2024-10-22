using System.Diagnostics.CodeAnalysis;

namespace Gateways.Cognito.Configurations
{
    [ExcludeFromCodeCoverage]
    public class CognitoConfig(string clientId, string clientSecret, string userPoolId) : ICognitoConfig
    {
        public string ClientId { get; set; } = clientId;
        public string ClientSecret { get; set; } = clientSecret;
        public string UserPoolId { get; set; } = userPoolId;
    }

    public interface ICognitoConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserPoolId { get; set; }
    }
}