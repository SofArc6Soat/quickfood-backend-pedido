using Domain.Entities;
using FluentValidation.TestHelper;

namespace Domain.Tests.Validators
{
    public class ValidarPedidoItemTests
    {
        private readonly ValidarPedidoItem _validator;

        public ValidarPedidoItemTests() => _validator = new ValidarPedidoItem();

        [Fact]
        public void ValidarPedidoItem_DeveSerValido_QuandoTodosOsCamposEstaoCorretos()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), 10, 100.00m);
            var validator = new ValidarPedidoItem();

            // Act
            var result = validator.TestValidate(pedidoItem);

            // Assert
            result.ShouldNotHaveValidationErrorFor(pi => pi.ProdutoId);
            result.ShouldNotHaveValidationErrorFor(pi => pi.Quantidade);
            result.ShouldNotHaveValidationErrorFor(pi => pi.ValorUnitario);
        }

        [Fact]
        public void ValidarPedidoItem_DeveSerInvalido_QuandoProdutoIdNaoForValido()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.Empty, 10, 15.00m);

            // Act
            var result = _validator.TestValidate(pedidoItem);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProdutoId);
        }

        [Fact]
        public void ValidarPedidoItem_DeveSerInvalido_QuandoQuantidadeForZeroOuNegativa()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), 0, 15.00m);

            // Act
            var result = _validator.TestValidate(pedidoItem);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quantidade);
        }

        [Fact]
        public void ValidarPedidoItem_DeveSerInvalido_QuandoValorUnitarioForZeroOuNegativo()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), 10, -5.00m);

            // Act
            var result = _validator.TestValidate(pedidoItem);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ValorUnitario);
        }
    }
}