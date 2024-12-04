using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;
using Moq;
using UseCases;

namespace Application.Tests.UseCases;

public class PedidoUseCaseTests
{
    private readonly Mock<IPedidoGateway> _pedidoGatewayMock;
    private readonly Mock<IProdutoGateway> _produtoGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly PedidoUseCase _pedidoUseCase;

    public PedidoUseCaseTests()
    {
        _pedidoGatewayMock = new Mock<IPedidoGateway>();
        _produtoGatewayMock = new Mock<IProdutoGateway>();
        _notificadorMock = new Mock<INotificador>();
        _pedidoUseCase = new PedidoUseCase(_pedidoGatewayMock.Object, _produtoGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnFalse_When_PedidoIsNull()
    {
        // Arrange
        Pedido pedido = null;
        var itens = new List<PedidoListaItens>();
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _pedidoUseCase.CadastrarPedidoAsync(pedido, itens, cancellationToken));
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnFalse_When_PedidoAlreadyExists()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid());
        var itens = new List<PedidoListaItens>();
        var cancellationToken = CancellationToken.None;

        _pedidoGatewayMock.Setup(x => x.VerificarPedidoExistenteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _pedidoUseCase.CadastrarPedidoAsync(pedido, itens, cancellationToken);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnFalse_When_PedidoIsInvalid()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid());
        var itens = new List<PedidoListaItens>();
        var cancellationToken = CancellationToken.None;

        _pedidoGatewayMock.Setup(x => x.VerificarPedidoExistenteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _notificadorMock.Setup(x => x.TemNotificacao()).Returns(true);

        // Act
        var result = await _pedidoUseCase.CadastrarPedidoAsync(pedido, itens, cancellationToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnFalse_When_PedidoHasNoItems()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid());
        var itens = new List<PedidoListaItens>();
        var cancellationToken = CancellationToken.None;

        _pedidoGatewayMock.Setup(x => x.VerificarPedidoExistenteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _pedidoUseCase.CadastrarPedidoAsync(pedido, itens, cancellationToken);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnTrue_When_PedidoIsValid()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid());
        var produto = new Produto(Guid.NewGuid(), "Produto Teste", "Descrição Teste", 100.0m, "Categoria Teste", true);
        var itens = new List<PedidoListaItens> { new PedidoListaItens(produto.Id, 1) };
        var cancellationToken = CancellationToken.None;

        _pedidoGatewayMock.Setup(x => x.VerificarPedidoExistenteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _produtoGatewayMock.Setup(x => x.ObterProdutoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(produto);
        _pedidoGatewayMock.Setup(x => x.CadastrarPedidoAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _pedidoUseCase.CadastrarPedidoAsync(pedido, itens, cancellationToken);

        // Assert
        Assert.True(result);
        _pedidoGatewayMock.Verify(x => x.CadastrarPedidoAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosAsync_Should_ReturnPedidos()
    {
        // Arrange
        var expectedPedidos = "Pedidos";
        var cancellationToken = CancellationToken.None;

        _pedidoGatewayMock.Setup(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedPedidos);

        // Act
        var result = await _pedidoUseCase.ObterTodosPedidosAsync(cancellationToken);

        // Assert
        Assert.Equal(expectedPedidos, result);
    }

    [Fact]
    public async Task ObterTodosPedidosOrdenadosAsync_Should_ReturnPedidosOrdenados()
    {
        // Arrange
        var expectedPedidosOrdenados = "Pedidos Ordenados";
        var cancellationToken = CancellationToken.None;

        _pedidoGatewayMock.Setup(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedPedidosOrdenados);

        // Act
        var result = await _pedidoUseCase.ObterTodosPedidosOrdenadosAsync(cancellationToken);

        // Assert
        Assert.Equal(expectedPedidosOrdenados, result);
    }
}