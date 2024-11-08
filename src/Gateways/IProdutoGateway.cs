using Domain.Entities;

namespace Gateways
{
    public interface IProdutoGateway
    {
        Task<Produto?> ObterProdutoAsync(Guid id, CancellationToken cancellationToken);

        Task<bool> CadastrarProdutoAsync(Produto produto, CancellationToken cancellationToken);
        Task<bool> AtualizarProdutoAsync(Produto produto, CancellationToken cancellationToken);
        Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken);
    }
}
