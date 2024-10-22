using Domain.Entities;
using Infra.Repositories;

namespace Gateways
{
    public class ProdutoGateway(IProdutoRepository produtoRepository) : IProdutoGateway
    {
        public async Task<Produto?> ObterProdutoAsync(Guid id, CancellationToken cancellationToken)
        {
            var produtoDto = await produtoRepository.FindByIdAsync(id, cancellationToken);

            return produtoDto is null
                ? null
                : new Produto(produtoDto.Id, produtoDto.Nome, produtoDto.Descricao, produtoDto.Preco, produtoDto.Categoria, produtoDto.Ativo);
        }
    }
}
