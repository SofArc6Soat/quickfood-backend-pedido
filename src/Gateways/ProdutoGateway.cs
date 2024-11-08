using Domain.Entities;
using Infra.Dto;
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

        public async Task<bool> CadastrarProdutoAsync(Produto produto, CancellationToken cancellationToken)
        {
            var produtoDto = new ProdutoDb
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria.ToString(),
                Ativo = produto.Ativo
            };

            await produtoRepository.InsertAsync(produtoDto, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> AtualizarProdutoAsync(Produto produto, CancellationToken cancellationToken)
        {
            var produtoDto = new ProdutoDb
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria.ToString(),
                Ativo = produto.Ativo
            };

            await produtoRepository.UpdateAsync(produtoDto, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken)
        {
            await produtoRepository.DeleteAsync(id, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }
    }
}
