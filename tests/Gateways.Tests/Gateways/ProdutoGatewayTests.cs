using Core.Infra.MessageBroker;
using Domain.Tests.TestHelpers;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using Worker.Dtos.Events;

namespace Gateways.Tests.Gateways;

public class ProdutoGatewayTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly ProdutoGateway _produtoGateway;

    public ProdutoGatewayTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();

        _produtoGateway = new ProdutoGateway(
            _produtoRepositoryMock.Object);
    }

    [Fact]
    public async Task ObterProdutoAsync_DeveRetornarProduto_QuandoProdutoExistir()
    {
        // Arrange
        var produtoDb = ProdutoFakeDataFactory.CriarProdutoDbValido();

        _produtoRepositoryMock.Setup(x => x.FindByIdAsync(produtoDb.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtoDb);

        // Act
        var result = await _produtoGateway.ObterProdutoAsync(produtoDb.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(produtoDb.Id, result.Id);
        Assert.Equal(produtoDb.Nome, result.Nome);
        Assert.Equal(produtoDb.Descricao, result.Descricao);
        Assert.Equal(produtoDb.Preco, result.Preco);
        Assert.Equal(produtoDb.Categoria, result.Categoria);
        Assert.Equal(produtoDb.Ativo, result.Ativo);
    }

    [Fact]
    public async Task ObterProdutoAsync_DeveRetornarNull_QuandoProdutoNaoExistir()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid3();

        _produtoRepositoryMock.Setup(x => x.FindByIdAsync(produtoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProdutoDb)null);

        // Act
        var result = await _produtoGateway.ObterProdutoAsync(produtoId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
