﻿using Core.Infra.MessageBroker;
using Domain.Entities;
using Gateways.Dtos.Events;
using Infra.Dto;
using Infra.Repositories;

namespace Gateways
{
    public class PedidoGateway(IPedidoRepository pedidoRepository, ISqsService<PedidoCriadoEvent> sqsPedidoCriado) : IPedidoGateway
    {
        public async Task<bool> CadastrarPedidoAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            var pedidoDto = new PedidoDb
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                ClienteId = pedido.ClienteId,
                Status = pedido.Status.ToString(),
                ValorTotal = pedido.ValorTotal,
                DataPedido = pedido.DataPedido
            };

            foreach (var item in pedido.PedidoItems)
            {
                pedidoDto.Itens.Add(new PedidoItemDb
                {
                    PedidoId = pedidoDto.Id,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario
                });
            }

            await pedidoRepository.InsertAsync(pedidoDto, cancellationToken);

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken) && await sqsPedidoCriado.SendMessageAsync(GerarPedidoCriadoEvent(pedidoDto));
        }

        public async Task<bool> VerificarPedidoExistenteAsync(Guid id, CancellationToken cancellationToken)
        {
            var pedidoExistente = await pedidoRepository.FindByIdAsync(id, cancellationToken);

            return pedidoExistente is not null;
        }

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken) =>
            await pedidoRepository.ObterTodosPedidosOrdenadosAsync(cancellationToken);

        public async Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken) =>
            await pedidoRepository.ObterTodosPedidosAsync(cancellationToken);

        private static PedidoCriadoEvent GerarPedidoCriadoEvent(PedidoDb pedidoDb) => new()
        {
            Id = pedidoDb.Id,
            NumeroPedido = pedidoDb.NumeroPedido,
            ClienteId = pedidoDb.ClienteId,
            Status = pedidoDb.Status,
            ValorTotal = pedidoDb.ValorTotal,
            DataPedido = pedidoDb.DataPedido
        };
    }
}
