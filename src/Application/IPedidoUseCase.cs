using Domain.Entities;
using Domain.ValueObjects;

namespace UseCases
{
    public interface IPedidoUseCase
    {
        Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken);
        Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken);
        Task<bool> CadastrarPedidoAsync(Pedido pedido, List<PedidoListaItens> itens, CancellationToken cancellationToken);
        Task<bool> AlterarStatusAsync(Guid pedidoId, PedidoStatus pedidoStatus, CancellationToken cancellationToken);
    }
}
