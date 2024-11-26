using Core.Domain.Data;
using Domain.Tests.TestHelpers;
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
                ProdutoFakeDataFactory.CriarProdutoDbValido(),
                ProdutoFakeDataFactory.CriarProdutoDbValido2()
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

    [Fact]
    public async Task FindByIdAsync_DeveRetornarProdutoPorId()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockProdutoRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtoDb);

        // Act
        var resultado = await _mockProdutoRepository.Object.FindByIdAsync(produtoDb.Id, cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(produtoDb.Id, resultado.Id);
        _mockProdutoRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();
        var cancellationToken = CancellationToken.None;

        _mockProdutoRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao encontrar produto por ID"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockProdutoRepository.Object.FindByIdAsync(produtoId, cancellationToken));
        Assert.Equal("Erro ao encontrar produto por ID", exception.Message);
        _mockProdutoRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveInserirProduto()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockProdutoRepository.Setup(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockProdutoRepository.Object.InsertAsync(produtoDb, cancellationToken);

        // Assert
        _mockProdutoRepository.Verify(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockProdutoRepository.Setup(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao inserir produto"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockProdutoRepository.Object.InsertAsync(produtoDb, cancellationToken));
        Assert.Equal("Erro ao inserir produto", exception.Message);
        _mockProdutoRepository.Verify(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveDeletarProduto()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockProdutoRepository.Setup(x => x.DeleteAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockProdutoRepository.Object.DeleteAsync(produtoDb, cancellationToken);

        // Assert
        _mockProdutoRepository.Verify(x => x.DeleteAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockProdutoRepository.Setup(x => x.DeleteAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao deletar produto"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockProdutoRepository.Object.DeleteAsync(produtoDb, cancellationToken));
        Assert.Equal("Erro ao deletar produto", exception.Message);
        _mockProdutoRepository.Verify(x => x.DeleteAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
