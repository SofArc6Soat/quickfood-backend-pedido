using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;

namespace Api.Tests.Controllers;

public class PedidosApiControllerTests
{
    private readonly Mock<IPedidoController> _pedidoControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly PedidosApiController _pedidosApiController;

    public PedidosApiControllerTests()
    {
        _pedidoControllerMock = new Mock<IPedidoController>();
        _notificadorMock = new Mock<INotificador>();
        _pedidosApiController = new PedidosApiController(_pedidoControllerMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task ObterTodosPedidos_Should_ReturnOkResult_WithPedidos()
    {
        // Arrange
        var expectedPedidos = JsonSerializer.Serialize(new { Message = "Pedidos JSON" });
        _pedidoControllerMock.Setup(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedPedidos);

        // Act
        var result = await _pedidosApiController.ObterTodosPedidos(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(expectedPedidos, JsonSerializer.Serialize(response.Data));
        _pedidoControllerMock.Verify(x => x.ObterTodosPedidosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodosPedidosOrdenados_Should_ReturnOkResult_WithPedidosOrdenados()
    {
        // Arrange
        var expectedPedidosOrdenados = JsonSerializer.Serialize(new { Message = "Pedidos Ordenados JSON" });
        _pedidoControllerMock.Setup(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedPedidosOrdenados);

        // Act
        var result = await _pedidosApiController.ObterTodosPedidosOrdenados(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(expectedPedidosOrdenados, JsonSerializer.Serialize(response.Data));
        _pedidoControllerMock.Verify(x => x.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarPedido_Should_ReturnCreatedResult_When_Successful()
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

        _pedidoControllerMock.Setup(x => x.CadastrarPedidoAsync(It.IsAny<PedidoRequestDto>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(true);

        // Act
        var result = await _pedidosApiController.CadastrarPedido(pedidoDto, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal($"pedidos/{pedidoDto.PedidoId}", createdResult.Location);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
        Assert.Equal(pedidoDto, response.Data);
        _pedidoControllerMock.Verify(x => x.CadastrarPedidoAsync(It.IsAny<PedidoRequestDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarPedido_Should_ReturnBadRequest_When_ModelStateIsInvalid()
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

        _pedidosApiController.ModelState.AddModelError("Error", "Model state is invalid");

        // Act
        var result = await _pedidosApiController.CadastrarPedido(pedidoDto, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        _pedidoControllerMock.Verify(x => x.CadastrarPedidoAsync(It.IsAny<PedidoRequestDto>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
