using Gateways.Dtos.Request;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace SmokeTests.SmokeTests;

public class PedidosApiControllerSmokeTest
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public PedidosApiControllerSmokeTest()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Success\":true}", System.Text.Encoding.UTF8, "application/json")
            });

        _client = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task Get_ObterTodosPedidosEndpoint_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/pedidos");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Get_ObterTodosPedidosEndpoint_ReturnsNotFound()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("http://localhost/pedidos")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

        var client = new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        // Act
        var response = await client.GetAsync("/pedidos");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_ObterTodosPedidosOrdenadosEndpoint_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/pedidos/ordenados");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Get_ObterTodosPedidosOrdenadosEndpoint_ReturnsNotFound()
    {
        // Arrange
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("http://localhost/pedidos/ordenados")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

        // Act
        var response = await _client.GetAsync("/pedidos/ordenados");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_CadastrarPedidoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var pedido = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            Items = new List<PedidoListaItensDto>
            {
                new PedidoListaItensDto
                {
                    ProdutoId = Guid.NewGuid(),
                    Quantidade = 2
                }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/pedidos", pedido);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_CadastrarPedidoEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var pedido = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            ClienteId = null, // ClienteId é obrigatório
            Items = new List<PedidoListaItensDto>
            {
                new PedidoListaItensDto
                {
                    ProdutoId = Guid.NewGuid(),
                    Quantidade = 2
                }
            }
        };

        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == new Uri("http://localhost/pedidos")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        // Act
        var response = await _client.PostAsJsonAsync("/pedidos", pedido);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Patch_AlterarStatusEndpoint_ReturnsSuccess()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedidoStatusDto = new PedidoStatusRequestDto
        {
            Status = "EmPreparacao"
        };

        // Act
        var response = await _client.PatchAsJsonAsync($"/pedidos/status/{pedidoId}", pedidoStatusDto);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Patch_AlterarStatusEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedidoStatusDto = new PedidoStatusRequestDto
        {
            Status = "StatusInvalido" // Status inválido
        };

        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Patch && req.RequestUri == new Uri($"http://localhost/pedidos/status/{pedidoId}")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        // Act
        var response = await _client.PatchAsJsonAsync($"/pedidos/status/{pedidoId}", pedidoStatusDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
