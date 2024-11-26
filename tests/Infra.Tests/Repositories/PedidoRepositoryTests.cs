using FluentAssertions;
using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories
{
    public class PedidoRepositoryTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;

        public PedidoRepositoryTests() => _pedidoRepositoryMock = new Mock<IPedidoRepository>();

        [Fact]
        public async Task ObterTodosPedidosOrdenadosAsync_DeveRetornarPedidosOrdenados()
        {
            // Arrange
            var pedidos = "Pedido1, Pedido2";
            _pedidoRepositoryMock.Setup(repo => repo.ObterTodosPedidosOrdenadosAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoRepositoryMock.Object.ObterTodosPedidosOrdenadosAsync(CancellationToken.None);

            // Assert
            resultado.Should().Be(pedidos);
        }

        [Fact]
        public async Task ObterTodosPedidosAsync_DeveRetornarTodosPedidos()
        {
            // Arrange
            var pedidos = "Pedido1, Pedido2";
            _pedidoRepositoryMock.Setup(repo => repo.ObterTodosPedidosAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoRepositoryMock.Object.ObterTodosPedidosAsync(CancellationToken.None);

            // Assert
            resultado.Should().Be(pedidos);
        }
    }
}