using System.Diagnostics.CodeAnalysis;

namespace Api.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class EnvironmentConfig
    {
        public static Settings ConfigureEnvironment(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new Settings();
            ConfigurationBinder.Bind(configuration, settings);

            services.AddSingleton<ICognitoSettings>(settings.CognitoSettings);

            return settings;
        }
    }

    [ExcludeFromCodeCoverage]
    public record Settings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();

        public CognitoSettings CognitoSettings { get; set; } = new CognitoSettings();

        public AwsSqsSettings AwsSqsSettings { get; set; } = new AwsSqsSettings();
    }

    [ExcludeFromCodeCoverage]
    public record ConnectionStrings
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public record AwsSqsSettings
    {
        public string QueueProdutoCriadoEvent { get; set; } = string.Empty;
        public string QueueProdutoAtualizadoEvent { get; set; } = string.Empty;
        public string QueueProdutoExcluidoEvent { get; set; } = string.Empty;
        public string QueuePedidoCriadoPagamentoEvent { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class CognitoSettings : ICognitoSettings
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string UserPoolId { get; set; } = string.Empty;
        public string Authority { get; set; } = string.Empty;
        public string MetadataAddress { get; set; } = string.Empty;
    }

    public interface ICognitoSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserPoolId { get; set; }
        public string Authority { get; set; }
        public string MetadataAddress { get; set; }
    }
}