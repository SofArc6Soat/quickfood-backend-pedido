using Core.Infra.MessageBroker;
using Domain.Entities;
using Gateways;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Worker.Dtos.Events;

namespace Worker
{
    public class ProdutoCriadoBackgroundService(ISqsService<ProdutoCriadoEvent> sqsClient, IServiceScopeFactory serviceScopeFactory, ILogger<ProdutoCriadoBackgroundService> logger) : BackgroundService
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

        private async Task ProcessMessageAsync(ProdutoCriadoEvent? message, CancellationToken cancellationToken)
        {
            if (message is not null)
            {
                using var scope = serviceScopeFactory.CreateScope();
                var produtoGateway = scope.ServiceProvider.GetRequiredService<IProdutoGateway>();
                await produtoGateway.CadastrarProdutoAsync(ConvertMessageToEntity(message), cancellationToken);
            }
        }

        private static Produto ConvertMessageToEntity(ProdutoCriadoEvent message) =>
            new(message.Id, message.Nome, message.Descricao, message.Preco, message.Categoria.ToString(), message.Ativo);
    }
}
