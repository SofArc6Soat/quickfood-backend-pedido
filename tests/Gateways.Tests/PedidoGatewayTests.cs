﻿using Core.Infra.MessageBroker;
using Domain.Tests.TestHelpers;
using Gateways.Dtos.Events;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Gateways.Tests;

public class PedidoGatewayTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<ISqsService<PedidoCriadoEvent>> _sqsPedidoCriadoMock;
    private readonly PedidoGateway _pedidoGateway;

    public PedidoGatewayTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _sqsPedidoCriadoMock = new Mock<ISqsService<PedidoCriadoEvent>>();

        _pedidoGateway = new PedidoGateway(
            _pedidoRepositoryMock.Object,
            _sqsPedidoCriadoMock.Object);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_DeveCadastrarPedidoEEnviarEventoComSucesso()
    {
        // Arrange
        var pedido = PedidoFakeDataFactory.CriarPedidoValido();

        _pedidoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _pedidoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _sqsPedidoCriadoMock.Setup(x => x.SendMessageAsync(It.IsAny<PedidoCriadoEvent>()))
            .ReturnsAsync(true);

        // Act
        var result = await _pedidoGateway.CadastrarPedidoAsync(pedido, CancellationToken.None);

        // Assert
        Assert.True(result);

        _pedidoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        _sqsPedidoCriadoMock.Verify(x => x.SendMessageAsync(It.IsAny<PedidoCriadoEvent>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_DeveRetornarMensagemErro_QuandoFalharAoInserirPedido()
    {
        // Arrange
        var pedido = PedidoFakeDataFactory.CriarPedidoInvalido();
        var mensagemErroEsperada = "Erro ao inserir pedido no banco de dados";

        _pedidoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagemErroEsperada));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _pedidoGateway.CadastrarPedidoAsync(pedido, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagemErroEsperada, exception.Message);

        _pedidoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        _sqsPedidoCriadoMock.Verify(x => x.SendMessageAsync(It.IsAny<PedidoCriadoEvent>()), Times.Never);
    }

    [Fact]
    public async Task AtualizarPedidoAsync_DeveAtualizarPedidoComSucesso()
    {
        // Arrange
        var pedido = PedidoFakeDataFactory.CriarPedidoValido();

        _pedidoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _pedidoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _pedidoGateway.AtualizarPedidoAsync(pedido, CancellationToken.None);

        // Assert
        Assert.True(result);

        _pedidoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarPedidoAsync_DeveRetornarMensagemErro_QuandoFalharAoAtualizarPedido()
    {
        // Arrange
        var pedido = PedidoFakeDataFactory.CriarPedidoValido();
        var mensagemErroEsperada = "Erro ao atualizar pedido no banco de dados";

        _pedidoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagemErroEsperada));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _pedidoGateway.AtualizarPedidoAsync(pedido, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagemErroEsperada, exception.Message);

        _pedidoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ObterPedidoAsync_DeveRetornarPedido_QuandoPedidoExistir()
    {
        // Arrange
        var pedidoId = PedidoFakeDataFactory.ObterGuid();
        var pedidoDb = PedidoFakeDataFactory.CriarPedidoDbValido();

        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedidoDb);

        // Act
        var result = await _pedidoGateway.ObterPedidoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pedidoDb.Id, result.Id);
    }

    [Fact]
    public async Task ObterPedidoAsync_DeveRetornarNull_QuandoPedidoNaoExistir()
    {
        // Arrange
        var pedidoId = PedidoFakeDataFactory.ObterGuid();

        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PedidoDb)null);

        // Act
        var result = await _pedidoGateway.ObterPedidoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task VerificarPedidoExistenteAsync_DeveRetornarTrue_QuandoPedidoExistir()
    {
        // Arrange
        var pedidoId = PedidoFakeDataFactory.ObterGuid();
        var pedidoDb = PedidoFakeDataFactory.CriarPedidoDbValido();

        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedidoDb);

        // Act
        var result = await _pedidoGateway.VerificarPedidoExistenteAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerificarPedidoExistenteAsync_DeveRetornarFalse_QuandoPedidoNaoExistir()
    {
        // Arrange
        var pedidoId = PedidoFakeDataFactory.ObterGuid();

        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PedidoDb)null);

        // Act
        var result = await _pedidoGateway.VerificarPedidoExistenteAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}