using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities
{
    public class PedidoItemTests
    {
        [Fact]
        public void AdicionarUnidades_DeveAdicionarUnidadesCorretamente()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), 5, 10.00m);

            // Act
            pedidoItem.AdicionarUnidades(3);

            // Assert
            pedidoItem.Quantidade.Should().Be(8);
        }
        [Fact]
        public void AdicionarUnidades_DeveLancarArgumentOutOfRangeException_QuandoUnidadesMenorOuIgualAZero()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), 1, 10.00m);

            // Act
            var exception = Record.Exception(() => pedidoItem.AdicionarUnidades(0));

            // Assert
            exception.Should().BeOfType<ArgumentOutOfRangeException>()
                .Which.Message.Should().Contain("O número de unidades a serem adicionadas deve ser maior que zero.")
                .And.Contain("Parameter 'unidades'");
        }

        [Fact]
        public void CalcularValor_DeveCalcularValorCorretamente()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), 5, 20.00m);

            // Act
            var valor = pedidoItem.CalcularValor();

            // Assert
            valor.Should().Be(100.00m);
        }
    }
}