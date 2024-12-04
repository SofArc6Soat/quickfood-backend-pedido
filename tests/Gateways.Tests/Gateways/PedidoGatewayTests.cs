using Core.Infra.MessageBroker;
using Domain.Entities;
using Domain.Tests.TestHelpers;
using Domain.ValueObjects;
using Gateways.Dtos.Events;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Gateways.Tests.Gateways;

public class PedidoGatewayTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<ISqsService<PedidoCriadoEvent>> _sqsServiceMock;
    private readonly PedidoGateway _pedidoGateway;

    public PedidoGatewayTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _sqsServiceMock = new Mock<ISqsService<PedidoCriadoEvent>>();
        _pedidoGateway = new PedidoGateway(_pedidoRepositoryMock.Object, _sqsServiceMock.Object);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnTrue_When_Successful()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid(), 123, Guid.NewGuid(), PedidoStatus.Recebido, 100.0m, DateTime.UtcNow);
        pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), 2, 50.0m));
        var pedidoDb = new PedidoDb
        {
            Id = pedido.Id,
            NumeroPedido = pedido.NumeroPedido,
            ClienteId = pedido.ClienteId,
            Status = pedido.Status.ToString(),
            ValorTotal = pedido.ValorTotal,
            DataPedido = pedido.DataPedido,
            Itens = new List<PedidoItemDb>()
        };

        _pedidoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _pedidoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _sqsServiceMock.Setup(x => x.SendMessageAsync(It.IsAny<PedidoCriadoEvent>())).ReturnsAsync(true);

        // Act
        var result = await _pedidoGateway.CadastrarPedidoAsync(pedido, CancellationToken.None);

        // Assert
        Assert.True(result);
        _pedidoRepositoryMock.Verify(x => x.InsertAsync(It.Is<PedidoDb>(p => p.Itens.Count == 1 && p.Itens[0].Quantidade == 2), It.IsAny<CancellationToken>()), Times.Once);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _sqsServiceMock.Verify(x => x.SendMessageAsync(It.IsAny<PedidoCriadoEvent>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnFalse_When_CommitFails()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid(), 123, Guid.NewGuid(), PedidoStatus.Recebido, 100.0m, DateTime.UtcNow);
        pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), 2, 50.0m));

        _pedidoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _pedidoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _pedidoGateway.CadastrarPedidoAsync(pedido, CancellationToken.None);

        // Assert
        Assert.False(result);
        _pedidoRepositoryMock.Verify(x => x.InsertAsync(It.Is<PedidoDb>(p => p.Itens.Count == 1 && p.Itens[0].Quantidade == 2), It.IsAny<CancellationToken>()), Times.Once);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _sqsServiceMock.Verify(x => x.SendMessageAsync(It.IsAny<PedidoCriadoEvent>()), Times.Never);
    }

    [Fact]
    public async Task VerificarPedidoExistenteAsync_Should_ReturnTrue_When_PedidoExists()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedidoDb = new PedidoDb { Id = pedidoId };

        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync(pedidoDb);

        // Act
        var result = await _pedidoGateway.VerificarPedidoExistenteAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.True(result);
        _pedidoRepositoryMock.Verify(x => x.FindByIdAsync(pedidoId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task VerificarPedidoExistenteAsync_Should_ReturnFalse_When_PedidoDoesNotExist()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();

        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync((PedidoDb)null);

        // Act
        var result = await _pedidoGateway.VerificarPedidoExistenteAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _pedidoRepositoryMock.Verify(x => x.FindByIdAsync(pedidoId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosAsync_Should_ReturnPedidos()
    {
        // Arrange
        var expectedPedidos = "Pedidos JSON";
        _pedidoRepositoryMock.Setup(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedPedidos);

        // Act
        var result = await _pedidoGateway.ObterTodosPedidosAsync(CancellationToken.None);

        // Assert
        Assert.Equal(expectedPedidos, result);
        _pedidoRepositoryMock.Verify(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosOrdenadosAsync_Should_ReturnPedidosOrdenados()
    {
        // Arrange
        var expectedPedidosOrdenados = "Pedidos Ordenados JSON";
        _pedidoRepositoryMock.Setup(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedPedidosOrdenados);

        // Act
        var result = await _pedidoGateway.ObterTodosPedidosOrdenadosAsync(CancellationToken.None);

        // Assert
        Assert.Equal(expectedPedidosOrdenados, result);
        _pedidoRepositoryMock.Verify(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
