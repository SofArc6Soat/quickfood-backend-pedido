using Core.Infra.MessageBroker;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Worker.Dtos.Events;

namespace Worker.BackgroundServices
{
    public class PedidoPedentePgtoBackgroundService(ISqsService<PedidoPendentePagamentoEvent> sqsClient, IServiceScopeFactory serviceScopeFactory, ILogger<PedidoPedentePgtoBackgroundService> logger) : BackgroundService
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

        private async Task ProcessMessageAsync(PedidoPendentePagamentoEvent? message, CancellationToken cancellationToken)
        {
            if (message is not null && message.Status.Equals("PendentePagamento"))
            {
                using var scope = serviceScopeFactory.CreateScope();
                var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

                var pedidoExistente = await pedidoRepository.FindByIdAsync(message.PedidoId, cancellationToken);

                if (pedidoExistente is not null)
                {
                    pedidoExistente.Status = "PendentePagamento";
                    await pedidoRepository.UpdateAsync(pedidoExistente, cancellationToken);
                    await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
                }
            }
        }
    }
}
