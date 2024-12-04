using FluentValidation;
using FluentValidation.TestHelper;
using Gateways.Dtos.Request;

namespace Gateways.Tests.Dtos.Request
{
    public class PedidoStatusRequestDtoValidator : AbstractValidator<PedidoStatusRequestDto>
    {
        private static readonly string[] AllowedStatuses = { "EmPreparacao", "Pronto", "Finalizado" };

        public PedidoStatusRequestDtoValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .Must(status => AllowedStatuses.Contains(status)).WithMessage("Status inválido.");
        }
    }

    public class PedidoStatusRequestDtoTests
    {
        private readonly PedidoStatusRequestDtoValidator _validator;

        public PedidoStatusRequestDtoTests()
        {
            _validator = new PedidoStatusRequestDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Null()
        {
            // Arrange
            var model = new PedidoStatusRequestDto { Status = null };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("O campo Status é obrigatório.");
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Empty()
        {
            // Arrange
            var model = new PedidoStatusRequestDto { Status = string.Empty };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("O campo Status é obrigatório.");
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Invalid()
        {
            // Arrange
            var model = new PedidoStatusRequestDto { Status = "Invalido" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("Status inválido.");
        }

        [Theory]
        [InlineData("EmPreparacao")]
        [InlineData("Pronto")]
        [InlineData("Finalizado")]
        public void Should_Not_Have_Error_When_Status_Is_Valid(string status)
        {
            // Arrange
            var model = new PedidoStatusRequestDto { Status = status };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }
    }
}