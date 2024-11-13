using Core.Infra.Repository;
using Infra.Context;
using Infra.Dto;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProdutoRepository(ApplicationDbContext context) : RepositoryGeneric<ProdutoDb>(context), IProdutoRepository
    {
        private readonly DbSet<ProdutoDb> _produtos = context.Set<ProdutoDb>();

        public async Task<IEnumerable<ProdutoDb>> ObterTodosProdutosAsync(CancellationToken cancellationToken) =>
            await _produtos.AsNoTracking().Where(p => p.Ativo).ToListAsync(cancellationToken);
    }
}
