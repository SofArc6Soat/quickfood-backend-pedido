using Gateways.Cognito.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Gateways.Cognito.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayCognitoDependencyServices(this IServiceCollection services, string clientId, string clientSecret, string userPoolId)
        {
            services.AddSingleton<ICognitoConfig>(new CognitoConfig(clientId, clientSecret, userPoolId));
            services.AddSingleton<ICognitoFactory>(new CognitoFactory(clientId, clientSecret, userPoolId));
            services.AddScoped<ICognitoGateway, CognitoGateway>();
        }
    }
}