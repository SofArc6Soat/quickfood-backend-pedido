using Domain.Entities;

namespace Gateways
{
    public interface IPedidoGateway
    {
        Task<bool> VerificarPedidoExistenteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> CadastrarPedidoAsync(Pedido pedido, CancellationToken cancellationToken);
        Task<Pedido?> ObterPedidoAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> AtualizarPedidoAsync(Pedido pedido, CancellationToken cancellationToken);

        Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken);
        Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken);
    }
}
