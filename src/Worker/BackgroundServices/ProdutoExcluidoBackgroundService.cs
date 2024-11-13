using Core.Infra.MessageBroker;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Worker.Dtos.Events;

namespace Worker.BackgroundServices
{
    public class ProdutoExcluidoBackgroundService(ISqsService<ProdutoExcluidoEvent> sqsClient, IServiceScopeFactory serviceScopeFactory, ILogger<ProdutoExcluidoBackgroundService> logger) : BackgroundService
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

        private async Task ProcessMessageAsync(ProdutoExcluidoEvent? message, CancellationToken cancellationToken)
        {
            if (message is not null)
            {
                using var scope = serviceScopeFactory.CreateScope();
                var produtoRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();

                var produtoExistente = await produtoRepository.FindByIdAsync(message.Id, cancellationToken);

                if (produtoExistente is not null)
                {
                    await produtoRepository.DeleteAsync(message.Id, cancellationToken);
                    await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
                }
            }
        }
    }
}
