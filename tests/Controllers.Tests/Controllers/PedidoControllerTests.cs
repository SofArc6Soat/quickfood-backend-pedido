using Domain.Entities;
using Gateways.Dtos.Request;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases;

namespace Controllers.Tests.Controllers;

public class PedidoControllerTests
{
    private readonly Mock<IPedidoUseCase> _pedidoUseCaseMock;
    private readonly PedidoController _pedidoController;

    public PedidoControllerTests()
    {
        _pedidoUseCaseMock = new Mock<IPedidoUseCase>();
        _pedidoController = new PedidoController(_pedidoUseCaseMock.Object);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnTrue_When_Successful()
    {
        // Arrange
        var pedidoDto = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            Items = new List<PedidoListaItensDto>
                {
                    new PedidoListaItensDto { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
                }
        };

        _pedidoUseCaseMock.Setup(x => x.CadastrarPedidoAsync(It.IsAny<Pedido>(), It.IsAny<List<PedidoListaItens>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);

        // Act
        var result = await _pedidoController.CadastrarPedidoAsync(pedidoDto, CancellationToken.None);

        // Assert
        Assert.True(result);
        _pedidoUseCaseMock.Verify(x => x.CadastrarPedidoAsync(It.IsAny<Pedido>(), It.IsAny<List<PedidoListaItens>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarPedidoAsync_Should_ReturnFalse_When_Fails()
    {
        // Arrange
        var pedidoDto = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            Items = new List<PedidoListaItensDto>
                {
                    new PedidoListaItensDto { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
                }
        };

        _pedidoUseCaseMock.Setup(x => x.CadastrarPedidoAsync(It.IsAny<Pedido>(), It.IsAny<List<PedidoListaItens>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(false);

        // Act
        var result = await _pedidoController.CadastrarPedidoAsync(pedidoDto, CancellationToken.None);

        // Assert
        Assert.False(result);
        _pedidoUseCaseMock.Verify(x => x.CadastrarPedidoAsync(It.IsAny<Pedido>(), It.IsAny<List<PedidoListaItens>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosAsync_Should_ReturnPedidos()
    {
        // Arrange
        var expectedPedidos = "Pedidos JSON";
        _pedidoUseCaseMock.Setup(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedPedidos);

        // Act
        var result = await _pedidoController.ObterTodosPedidosAsync(CancellationToken.None);

        // Assert
        Assert.Equal(expectedPedidos, result);
        _pedidoUseCaseMock.Verify(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosOrdenadosAsync_Should_ReturnPedidosOrdenados()
    {
        // Arrange
        var expectedPedidosOrdenados = "Pedidos Ordenados JSON";
        _pedidoUseCaseMock.Setup(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedPedidosOrdenados);

        // Act
        var result = await _pedidoController.ObterTodosPedidosOrdenadosAsync(CancellationToken.None);

        // Assert
        Assert.Equal(expectedPedidosOrdenados, result);
        _pedidoUseCaseMock.Verify(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
