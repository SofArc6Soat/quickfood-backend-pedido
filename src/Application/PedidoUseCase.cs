using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways;

namespace UseCases
{
    public class PedidoUseCase(IPedidoGateway pedidoGateway, IProdutoGateway produtoGateway, INotificador notificador) : BaseUseCase(notificador), IPedidoUseCase
    {
        public async Task<bool> CadastrarPedidoAsync(Pedido pedido, List<PedidoListaItens> itens, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(pedido);

            if (await pedidoGateway.VerificarPedidoExistenteAsync(pedido.Id, cancellationToken))
            {
                Notificar("Pedido já existente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarPedido(), pedido))
            {
                return false;
            }

            foreach (var item in itens)
            {
                var produto = await produtoGateway.ObterProdutoAsync(item.ProdutoId, cancellationToken);

                if (produto is null)
                {
                    Notificar($"Produto {item.ProdutoId} não encontrado.");
                }
                else
                {
                    pedido.AdicionarItem(new PedidoItem(item.ProdutoId, item.Quantidade, produto.Preco));
                }
            }

            if (pedido.PedidoItems.Count == 0)
            {
                Notificar("O pedido precisa ter pelo menos um item.");
                return false;
            }

            return await pedidoGateway.CadastrarPedidoAsync(pedido, cancellationToken);
        }

        public async Task<bool> AlterarStatusAsync(Guid pedidoId, PedidoStatus pedidoStatus, CancellationToken cancellationToken)
        {
            var pedido = await pedidoGateway.ObterPedidoAsync(pedidoId, cancellationToken);

            if (pedido is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return false;
            }

            if (!pedido.AlterarStatus(pedidoStatus))
            {
                Notificar("Não foi possível alterar o status do pedido.");
                return false;
            }

            return await pedidoGateway.AtualizarPedidoAsync(pedido, cancellationToken);
        }

        public async Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken) =>
            await pedidoGateway.ObterTodosPedidosAsync(cancellationToken);

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken) =>
            await pedidoGateway.ObterTodosPedidosOrdenadosAsync(cancellationToken);
    }
}
