using Domain.Entities;

namespace Gateways
{
    public interface IProdutoGateway
    {
        Task<Produto?> ObterProdutoAsync(Guid id, CancellationToken cancellationToken);
    }
}
