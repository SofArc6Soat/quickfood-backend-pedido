﻿using Domain.Entities;
using Gateways.Dtos.Request;
using UseCases;

namespace Controllers
{
    public class PedidoController(IPedidoUseCase pedidoUseCase) : IPedidoController
    {
        public async Task<bool> CadastrarPedidoAsync(PedidoRequestDto pedidoDto, CancellationToken cancellationToken)
        {
            var pedido = new Pedido(pedidoDto.PedidoId, pedidoDto.ClienteId);

            var pedidoListaItens = new List<PedidoListaItens>();

            foreach (var item in pedidoDto.Items)
            {
                pedidoListaItens.Add(new PedidoListaItens(item.ProdutoId, item.Quantidade));
            }

            return await pedidoUseCase.CadastrarPedidoAsync(pedido, pedidoListaItens, cancellationToken);
        }

        public async Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken) =>
            await pedidoUseCase.ObterTodosPedidosAsync(cancellationToken);

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken) =>
            await pedidoUseCase.ObterTodosPedidosOrdenadosAsync(cancellationToken);
    }
}