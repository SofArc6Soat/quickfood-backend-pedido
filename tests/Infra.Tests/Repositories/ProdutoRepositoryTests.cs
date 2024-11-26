using Core.Domain.Data;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories;

public class ProdutoRepositoryTests
{
    private readonly Mock<IProdutoRepository> _mockProdutoRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public ProdutoRepositoryTests()
    {
        _mockProdutoRepository = new Mock<IProdutoRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProdutoRepository.Setup(repo => repo.UnitOfWork).Returns(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task ObterTodosProdutosAsync_DeveRetornarListaDeProdutos()
    {
        // Arrange
        var produtosDb = new List<ProdutoDb>
            {
                new() { Id = Guid.NewGuid(), Nome = "Produto 1", Descricao = "Descricao 1", Preco = 10.0m, Categoria = "Categoria 1", Ativo = true },
                new() { Id = Guid.NewGuid(), Nome = "Produto 2", Descricao = "Descricao 2", Preco = 20.0m, Categoria = "Categoria 2", Ativo = true }
            };
        var cancellationToken = CancellationToken.None;

        _mockProdutoRepository.Setup(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtosDb);

        // Act
        var resultado = await _mockProdutoRepository.Object.ObterTodosProdutosAsync(cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        _mockProdutoRepository.Verify(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosProdutosAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _mockProdutoRepository.Setup(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao obter todos os produtos"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockProdutoRepository.Object.ObterTodosProdutosAsync(cancellationToken));
        Assert.Equal("Erro ao obter todos os produtos", exception.Message);
        _mockProdutoRepository.Verify(x => x.ObterTodosProdutosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}