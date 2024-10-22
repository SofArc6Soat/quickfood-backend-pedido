using Infra.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Gateways.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayDependencyServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IProdutoGateway, ProdutoGateway>();
            services.AddScoped<IPedidoGateway, PedidoGateway>();

            services.AddInfraDependencyServices(connectionString);
        }
    }
}