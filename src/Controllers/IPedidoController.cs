using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IPedidoController
    {
        Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken);
        Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken);
        Task<bool> CadastrarPedidoAsync(PedidoRequestDto pedidoDto, CancellationToken cancellationToken);
        Task<bool> AlterarStatusAsync(Guid pedidoId, PedidoStatusRequestDto pedidoStatusDto, CancellationToken cancellationToken);
    }
}
