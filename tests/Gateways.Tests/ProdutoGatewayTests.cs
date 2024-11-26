using Core.Infra.MessageBroker;
using Domain.Tests.TestHelpers;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using Worker.Dtos.Events;

namespace Gateways.Tests;

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
    public async Task CadastrarProdutoAsync_DeveCadastrarProdutoEEnviarEventoComSucesso()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoValido();

        _produtoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        

        // Act
        var result = await _produtoGateway.CadastrarProdutoAsync(produto, CancellationToken.None);

        // Assert
        Assert.True(result);

        _produtoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarProdutoAsync_DeveRetornarMensagemErro_QuandoFalharAoInserirProduto()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoInvalido();
        var mensagemErroEsperada = "Erro ao inserir produto no banco de dados";

        _produtoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagemErroEsperada));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _produtoGateway.CadastrarProdutoAsync(produto, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagemErroEsperada, exception.Message);

        _produtoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        
    }

    [Fact]
    public async Task AtualizarProdutoAsync_DeveAtualizarProdutoComSucesso()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoValido();

        _produtoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _produtoGateway.AtualizarProdutoAsync(produto, CancellationToken.None);

        // Assert
        Assert.True(result);

        _produtoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_DeveRetornarMensagemErro_QuandoFalharAoAtualizarProduto()
    {
        // Arrange
        var produto = ProdutoFakeDataFactory.CriarProdutoValido();
        var mensagemErroEsperada = "Erro ao atualizar produto no banco de dados";

        _produtoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagemErroEsperada));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _produtoGateway.AtualizarProdutoAsync(produto, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagemErroEsperada, exception.Message);

        _produtoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProdutoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        
    }

    [Fact]
    public async Task DeletarProdutoAsync_DeveDeletarProdutoComSucesso()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();

        _produtoRepositoryMock.Setup(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _produtoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _produtoGateway.DeletarProdutoAsync(produtoId, CancellationToken.None);

        // Assert
        Assert.True(result);

        _produtoRepositoryMock.Verify(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletarProdutoAsync_DeveRetornarMensagemErro_QuandoFalharAoDeletarProduto()
    {
        // Arrange
        var produtoId = ProdutoFakeDataFactory.ObterGuid();
        var mensagemErroEsperada = "Erro ao deletar produto no banco de dados";

        _produtoRepositoryMock.Setup(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagemErroEsperada));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _produtoGateway.DeletarProdutoAsync(produtoId, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagemErroEsperada, exception.Message);

        _produtoRepositoryMock.Verify(x => x.DeleteAsync(produtoId, It.IsAny<CancellationToken>()), Times.Once);
        _produtoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        
    }
}