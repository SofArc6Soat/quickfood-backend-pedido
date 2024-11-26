using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects
{
    public class PedidoStatusTests
    {
        [Fact]
        public void PedidoStatus_DeveConterTodosOsValoresDefinidos()
        {
            // Act
            var valores = Enum.GetValues(typeof(PedidoStatus)).Cast<PedidoStatus>();

            // Assert
            valores.Should().HaveCount(6) // O enum possui 6 valores
                .And.Contain(
                [
                    PedidoStatus.Rascunho,
                    PedidoStatus.PendentePagamento,
                    PedidoStatus.Recebido,
                    PedidoStatus.EmPreparacao,
                    PedidoStatus.Pronto,
                    PedidoStatus.Finalizado
                ]);
        }

        [Theory]
        [InlineData(PedidoStatus.Rascunho, 0)]
        [InlineData(PedidoStatus.PendentePagamento, 1)]
        [InlineData(PedidoStatus.Recebido, 2)]
        [InlineData(PedidoStatus.EmPreparacao, 3)]
        [InlineData(PedidoStatus.Pronto, 4)]
        [InlineData(PedidoStatus.Finalizado, 5)]
        public void PedidoStatus_DeveTerValoresCorretos(PedidoStatus status, int valorEsperado) =>
            // Assert
            ((int)status).Should().Be(valorEsperado);

        [Theory]
        [InlineData(0, PedidoStatus.Rascunho)]
        [InlineData(1, PedidoStatus.PendentePagamento)]
        [InlineData(2, PedidoStatus.Recebido)]
        [InlineData(3, PedidoStatus.EmPreparacao)]
        [InlineData(4, PedidoStatus.Pronto)]
        [InlineData(5, PedidoStatus.Finalizado)]
        public void PedidoStatus_DeveConverterDeInteiroCorretamente(int valor, PedidoStatus statusEsperado)
        {
            // Act
            var status = (PedidoStatus)valor;

            // Assert
            status.Should().Be(statusEsperado);
        }

        [Fact]
        public void PedidoStatus_InvalidoDeveGerarExcecao()
        {
            // Act
            Action act = () => Enum.Parse<PedidoStatus>("Invalido");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Invalido*");
        }
    }
}
