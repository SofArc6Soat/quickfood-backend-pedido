using Core.Domain.Data;
using Infra.Dto;

namespace Infra.Repositories
{
    public interface IPedidoRepository : IRepositoryGeneric<PedidoDb>
    {
        Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken);
        Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken);
    }
}
