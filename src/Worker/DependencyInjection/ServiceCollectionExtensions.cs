using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Core.Infra.MessageBroker;
using Core.Infra.MessageBroker.DependencyInjection;
using Worker.Dtos.Events;
using Amazon.SQS;

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
            //services.AddSingleton<ISqsService<ProdutoAtualizadoEvent>>(provider => new SqsService<ProdutoAtualizadoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueProdutoAtualizadoEvent));
            //services.AddSingleton<ISqsService<ProdutoExcluidoEvent>>(provider => new SqsService<ProdutoExcluidoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueProdutoExcluidoEvent));
            //services.AddSingleton<ISqsService<ClienteCriadoEvent>>(provider => new SqsService<ClienteCriadoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueClienteCriadoEvent));

            services.AddHostedService<ProdutoCriadoBackgroundService>();
        }
    }

    [ExcludeFromCodeCoverage]
    public record WorkerQueues
    {
        public string QueueProdutoCriadoEvent { get; set; } = string.Empty;
        public string QueueProdutoAtualizadoEvent { get; set; } = string.Empty;
        public string QueueProdutoExcluidoEvent { get; set; } = string.Empty;
        public string QueueClienteCriadoEvent { get; set; } = string.Empty;
    }
}