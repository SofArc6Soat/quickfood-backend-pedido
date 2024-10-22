using Core.Domain.Data;
using Infra.Dto;

namespace Infra.Repositories
{
    public interface IProdutoRepository : IRepositoryGeneric<ProdutoDb>
    {
        Task<IEnumerable<ProdutoDb>> ObterTodosProdutosAsync(CancellationToken cancellationToken);
    }
}
