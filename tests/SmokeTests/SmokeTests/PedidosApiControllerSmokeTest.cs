using Domain.Tests.TestHelpers;
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
        _handlerMock = MockHttpMessageHandler.SetupMessageHandlerMock(HttpStatusCode.OK, "{\"Success\":true}");
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
        _handlerMock.SetupRequest(HttpMethod.Get, "http://localhost/pedidos", HttpStatusCode.NotFound, "{\"Success\":false, \"Errors\":[\"Pedidos não encontrados\"]}");

        // Act
        var response = await _client.GetAsync("/pedidos");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Pedidos não encontrados", content);
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
        _handlerMock.SetupRequest(HttpMethod.Get, "http://localhost/pedidos/ordenados", HttpStatusCode.NotFound, "{\"Success\":false, \"Errors\":[\"Pedidos ordenados não encontrados\"]}");

        // Act
        var response = await _client.GetAsync("/pedidos/ordenados");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Pedidos ordenados não encontrados", content);
    }

    [Fact]
    public async Task Post_CadastrarPedidoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var pedido = PedidoFakeDataFactory.CriarPedidoValido();

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
        var pedido = PedidoFakeDataFactory.CriarPedidoInvalido();

        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/pedidos", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao cadastrar pedido\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/pedidos", pedido);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao cadastrar pedido", content);
    }

    [Fact]
    public async Task Patch_AlterarStatusEndpoint_ReturnsSuccess()
    {
        // Arrange
        var pedidoId = PedidoFakeDataFactory.ObterNovoGuid();
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
        var pedidoId = PedidoFakeDataFactory.ObterNovoGuid();
        var pedidoStatusDto = new PedidoStatusRequestDto
        {
            Status = "StatusInvalido" // Status inválido
        };

        _handlerMock.SetupRequest(HttpMethod.Patch, $"http://localhost/pedidos/status/{pedidoId}", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao alterar status\"]}");

        // Act
        var response = await _client.PatchAsJsonAsync($"/pedidos/status/{pedidoId}", pedidoStatusDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao alterar status", content);
    }
}
