using Core.Infra.MessageBroker;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Worker.Dtos.Events;

namespace Worker.BackgroundServices
{
    public class PedidoStatusAlteradoBackgroundService(ISqsService<PedidoStatusAlteradoEvent> sqsClient, IServiceScopeFactory serviceScopeFactory, ILogger<PedidoStatusAlteradoBackgroundService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessMessageAsync(await sqsClient.ReceiveMessagesAsync(stoppingToken), stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing messages.");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessMessageAsync(PedidoStatusAlteradoEvent? message, CancellationToken cancellationToken)
        {
            if (message is not null && !string.IsNullOrEmpty(message.Status))
            {
                using var scope = serviceScopeFactory.CreateScope();
                var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

                var pedidoExistente = await pedidoRepository.FindByIdAsync(message.Id, cancellationToken);

                if (pedidoExistente is not null)
                {
                    if (string.Equals(pedidoExistente.Status, "Recebido") && string.Equals(message.Status, "EmPreparacao"))
                    {
                        pedidoExistente.Status = message.Status;
                    }

                    if (string.Equals(pedidoExistente.Status, "EmPreparacao") && string.Equals(message.Status, "Pronto"))
                    {
                        pedidoExistente.Status = message.Status;
                    }

                    if (string.Equals(pedidoExistente.Status, "Pronto") && string.Equals(message.Status, "Finalizado"))
                    {
                        pedidoExistente.Status = message.Status;
                    }

                    await pedidoRepository.UpdateAsync(pedidoExistente, cancellationToken);

                    await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
                }
            }
        }
    }
}