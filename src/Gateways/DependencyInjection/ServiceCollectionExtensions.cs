using Amazon.SQS;
using Core.Infra.MessageBroker;
using Core.Infra.MessageBroker.DependencyInjection;
using Gateways.Dtos.Events;
using Infra.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Gateways.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayDependencyServices(this IServiceCollection services, string connectionString, Queues queues)
        {
            services.AddScoped<IProdutoGateway, ProdutoGateway>();
            services.AddScoped<IPedidoGateway, PedidoGateway>();

            services.AddInfraDependencyServices(connectionString);

            // AWS SQS
            services.AddAwsSqsMessageBroker();

            services.AddSingleton<ISqsService<PedidoCriadoEvent>>(provider => new SqsService<PedidoCriadoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueuePedidoCriadoPagamentoEvent));
        }
    }

    [ExcludeFromCodeCoverage]
    public record Queues
    {
        public string QueuePedidoCriadoPagamentoEvent { get; set; } = string.Empty;
    }
}