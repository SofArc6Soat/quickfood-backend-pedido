using Core.Infra.MessageBroker;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Worker.Dtos.Events;

namespace Worker.BackgroundServices
{
    public class PedidoPagoBackgroundService(ISqsService<PedidoPagoEvent> sqsClient, ISqsService<PedidoRecebidoEvent> sqsPedidoRecebido, IServiceScopeFactory serviceScopeFactory, ILogger<PedidoPagoBackgroundService> logger) : BackgroundService
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

        private async Task ProcessMessageAsync(PedidoPagoEvent? message, CancellationToken cancellationToken)
        {
            if (message is not null && message.Status.Equals("Pago"))
            {
                using var scope = serviceScopeFactory.CreateScope();
                var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

                var pedidoExistente = await pedidoRepository.Query().Include(x => x.Itens).ToListAsync(cancellationToken);

                if (pedidoExistente.Count > 0)
                {
                    var pedido = new PedidoDb
                    {
                        Id = pedidoExistente[0].Id,
                        NumeroPedido = pedidoExistente[0].NumeroPedido,
                        ClienteId = pedidoExistente[0].ClienteId,
                        Status = "Recebido",
                        ValorTotal = pedidoExistente[0].ValorTotal,
                        DataPedido = pedidoExistente[0].DataPedido
                    };

                    await pedidoRepository.UpdateAsync(pedido, cancellationToken);

                    if (await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken))
                    {
                        var itens = new List<PedidoItemEvent>();

                        foreach (var item in pedidoExistente[0].Itens)
                        {
                            itens.Add(new PedidoItemEvent(item.Id, item.PedidoId, item.ProdutoId, item.Quantidade, item.ValorUnitario));
                        }

                        var pedidoRecebidoEvent = new PedidoRecebidoEvent(pedido.Id, pedido.NumeroPedido, pedido.ClienteId, pedido.Status.ToString(), pedido.ValorTotal, pedido.DataPedido, itens);

                        await sqsPedidoRecebido.SendMessageAsync(pedidoRecebidoEvent);
                    }
                }
            }
        }

    }
}