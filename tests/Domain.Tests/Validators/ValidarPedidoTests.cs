using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace Domain.Tests.Validators
{
    public class ValidarPedidoTests
    {
        private readonly ValidarPedido _validator;

        public ValidarPedidoTests() => _validator = new ValidarPedido();

        [Fact]
        public void ValidarPedido_DeveSerValido_QuandoTodosOsCamposEstaoCorretos()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), 1, Guid.NewGuid(), PedidoStatus.Rascunho, 100.00m, DateTime.Now);

            // Act
            var result = _validator.TestValidate(pedido);

            // Assert
            result.ShouldNotHaveValidationErrorFor(p => p.Id);
            result.ShouldNotHaveValidationErrorFor(p => p.NumeroPedido);
            result.ShouldNotHaveValidationErrorFor(p => p.ClienteId);
            result.ShouldNotHaveValidationErrorFor(p => p.ValorTotal);
            result.ShouldNotHaveValidationErrorFor(p => p.DataPedido);
        }
    }
}