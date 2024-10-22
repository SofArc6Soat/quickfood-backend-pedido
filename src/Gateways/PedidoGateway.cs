using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;
using Infra.Repositories;

namespace Gateways
{
    public class PedidoGateway(IPedidoRepository pedidoRepository) : IPedidoGateway
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

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<Pedido?> ObterPedidoAsync(Guid id, CancellationToken cancellationToken)
        {
            var pedidoDto = await pedidoRepository.FindByIdAsync(id, cancellationToken);

            if (pedidoDto is null)
            {
                return null;
            }

            _ = Enum.TryParse(pedidoDto.Status, out PedidoStatus status);
            return new Pedido(pedidoDto.Id, pedidoDto.NumeroPedido, pedidoDto.ClienteId, status, pedidoDto.ValorTotal, pedidoDto.DataPedido);
        }

        public async Task<bool> VerificarPedidoExistenteAsync(Guid id, CancellationToken cancellationToken)
        {
            var pedidoExistente = await pedidoRepository.FindByIdAsync(id, cancellationToken);

            return pedidoExistente is not null;
        }

        public async Task<bool> AtualizarPedidoAsync(Pedido pedido, CancellationToken cancellationToken)
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

            await pedidoRepository.UpdateAsync(pedidoDto, cancellationToken);

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken) =>
            await pedidoRepository.ObterTodosPedidosOrdenadosAsync(cancellationToken);

        public async Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken) =>
            await pedidoRepository.ObterTodosPedidosAsync(cancellationToken);
    }
}
