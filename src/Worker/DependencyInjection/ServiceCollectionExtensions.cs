using Amazon.SQS;
using Core.Infra.MessageBroker;
using Core.Infra.MessageBroker.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Worker.BackgroundServices;
using Worker.Dtos.Events;

namespace Worker.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddWorkerDependencyServices(this IServiceCollection services, WorkerQueues queues)
        {
            // AWS SQS
            services.AddAwsSqsMessageBroker();

            services.AddSingleton<ISqsService<ProdutoCriadoEvent>>(provider => new SqsService<ProdutoCriadoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueProdutoCriadoEvent));
            services.AddSingleton<ISqsService<ProdutoAtualizadoEvent>>(provider => new SqsService<ProdutoAtualizadoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueProdutoAtualizadoEvent));
            services.AddSingleton<ISqsService<ProdutoExcluidoEvent>>(provider => new SqsService<ProdutoExcluidoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueProdutoExcluidoEvent));
            services.AddSingleton<ISqsService<PedidoPendentePagamentoEvent>>(provider => new SqsService<PedidoPendentePagamentoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueuePedidoPendentePagamentoEvent));
            services.AddSingleton<ISqsService<PedidoPagoEvent>>(provider => new SqsService<PedidoPagoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueuePedidoPagoEvent));
            services.AddSingleton<ISqsService<PedidoRecebidoEvent>>(provider => new SqsService<PedidoRecebidoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueuePedidoRecebidoEvent));
            services.AddSingleton<ISqsService<PedidoStatusAlteradoEvent>>(provider => new SqsService<PedidoStatusAlteradoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueuePedidoStatusAlteradoEvent));

            services.AddHostedService<ProdutoCriadoBackgroundService>();
            services.AddHostedService<ProdutoAtualizadoBackgroundService>();
            services.AddHostedService<ProdutoExcluidoBackgroundService>();
            services.AddHostedService<PedidoPedentePgtoBackgroundService>();
            services.AddHostedService<PedidoPagoBackgroundService>();
            services.AddHostedService<PedidoStatusAlteradoBackgroundService>();
        }
    }

    [ExcludeFromCodeCoverage]
    public record WorkerQueues
    {
        public string QueueProdutoCriadoEvent { get; set; } = string.Empty;
        public string QueueProdutoAtualizadoEvent { get; set; } = string.Empty;
        public string QueueProdutoExcluidoEvent { get; set; } = string.Empty;
        public string QueuePedidoPagoEvent { get; set; } = string.Empty;
        public string QueuePedidoPendentePagamentoEvent { get; set; } = string.Empty;
        public string QueuePedidoRecebidoEvent { get; set; } = string.Empty;
        public string QueuePedidoStatusAlteradoEvent { get; set; } = string.Empty;
    }
}