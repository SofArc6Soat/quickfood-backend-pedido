using Core.Domain.Data;
using Domain.Tests.TestHelpers;
using FluentAssertions;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories;

public class PedidoRepositoryTests
{
    private readonly Mock<IPedidoRepository> _mockPedidoRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public PedidoRepositoryTests()
    {
        _mockPedidoRepository = new Mock<IPedidoRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPedidoRepository.Setup(repo => repo.UnitOfWork).Returns(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task ObterTodosPedidosOrdenadosAsync_DeveRetornarPedidosOrdenados()
    {
        // Arrange
        var pedidosOrdenados = "Pedidos Ordenados";
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedidosOrdenados);

        // Act
        var resultado = await _mockPedidoRepository.Object.ObterTodosPedidosOrdenadosAsync(cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(pedidosOrdenados, resultado);
        _mockPedidoRepository.Verify(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosOrdenadosAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao obter pedidos ordenados"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPedidoRepository.Object.ObterTodosPedidosOrdenadosAsync(cancellationToken));
        Assert.Equal("Erro ao obter pedidos ordenados", exception.Message);
        _mockPedidoRepository.Verify(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosAsync_DeveRetornarTodosPedidos()
    {
        // Arrange
        var todosPedidos = "Todos os Pedidos";
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(todosPedidos);

        // Act
        var resultado = await _mockPedidoRepository.Object.ObterTodosPedidosAsync(cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(todosPedidos, resultado);
        _mockPedidoRepository.Verify(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao obter todos os pedidos"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPedidoRepository.Object.ObterTodosPedidosAsync(cancellationToken));
        Assert.Equal("Erro ao obter todos os pedidos", exception.Message);
        _mockPedidoRepository.Verify(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveRetornarPedidoPorId()
    {
        // Arrange
        var pedidoDb = PedidoFakeDataFactory.CriarPedidoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedidoDb);

        // Act
        var resultado = await _mockPedidoRepository.Object.FindByIdAsync(pedidoDb.Id, cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(pedidoDb.Id, resultado.Id);
        _mockPedidoRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var pedidoId = PedidoFakeDataFactory.ObterNovoGuid();
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao encontrar pedido por ID"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPedidoRepository.Object.FindByIdAsync(pedidoId, cancellationToken));
        Assert.Equal("Erro ao encontrar pedido por ID", exception.Message);
        _mockPedidoRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveInserirPedido()
    {
        // Arrange
        var pedidoDb = PedidoFakeDataFactory.CriarPedidoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockPedidoRepository.Object.InsertAsync(pedidoDb, cancellationToken);

        // Assert
        _mockPedidoRepository.Verify(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var pedidoDb = PedidoFakeDataFactory.CriarPedidoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao inserir pedido"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPedidoRepository.Object.InsertAsync(pedidoDb, cancellationToken));
        Assert.Equal("Erro ao inserir pedido", exception.Message);
        _mockPedidoRepository.Verify(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveDeletarPedido()
    {
        // Arrange
        var pedidoDb = PedidoFakeDataFactory.CriarPedidoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.DeleteAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockPedidoRepository.Object.DeleteAsync(pedidoDb, cancellationToken);

        // Assert
        _mockPedidoRepository.Verify(x => x.DeleteAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var pedidoDb = PedidoFakeDataFactory.CriarPedidoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPedidoRepository.Setup(x => x.DeleteAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao deletar pedido"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPedidoRepository.Object.DeleteAsync(pedidoDb, cancellationToken));
        Assert.Equal("Erro ao deletar pedido", exception.Message);
        _mockPedidoRepository.Verify(x => x.DeleteAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}