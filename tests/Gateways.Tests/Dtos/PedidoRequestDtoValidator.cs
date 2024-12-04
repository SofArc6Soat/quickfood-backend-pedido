using FluentValidation;
using FluentValidation.TestHelper;
using Gateways.Dtos.Request;

namespace Gateways.Tests.Dtos.Request;

public class PedidoRequestDtoValidator : AbstractValidator<PedidoRequestDto>
{
    public PedidoRequestDtoValidator()
    {
        RuleFor(x => x.PedidoId)
            .NotEmpty().WithMessage("O campo PedidoId é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O campo PedidoId é obrigatório.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("O campo Items é obrigatório.")
            .ForEach(item => item.SetValidator(new PedidoListaItensDtoValidator()));
    }
}

public class PedidoListaItensDtoValidator : AbstractValidator<PedidoListaItensDto>
{
    public PedidoListaItensDtoValidator()
    {
        RuleFor(x => x.ProdutoId)
            .NotEmpty().WithMessage("O campo ProdutoId é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O campo ProdutoId é obrigatório.");

        RuleFor(x => x.Quantidade)
            .NotEmpty().WithMessage("O campo Quantidade é obrigatório.")
            .InclusiveBetween(1, 9999).WithMessage("O campo Quantidade deve ter o valor entre 1 e 9999.");
    }
}

public class PedidoRequestDtoTests
{
    private readonly PedidoRequestDtoValidator _validator;

    public PedidoRequestDtoTests()
    {
        _validator = new PedidoRequestDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PedidoId_Is_Empty()
    {
        // Arrange
        var model = new PedidoRequestDto { PedidoId = Guid.Empty, Items = new List<PedidoListaItensDto>() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PedidoId)
            .WithErrorMessage("O campo PedidoId é obrigatório.");
    }

    [Fact]
    public void Should_Have_Error_When_Items_Is_Empty()
    {
        // Arrange
        var model = new PedidoRequestDto { PedidoId = Guid.NewGuid(), Items = new List<PedidoListaItensDto>() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items)
            .WithErrorMessage("O campo Items é obrigatório.");
    }

    [Fact]
    public void Should_Have_Error_When_ProdutoId_Is_Empty()
    {
        // Arrange
        var model = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            Items = new List<PedidoListaItensDto>
                {
                    new PedidoListaItensDto { ProdutoId = Guid.Empty, Quantidade = 1 }
                }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].ProdutoId")
            .WithErrorMessage("O campo ProdutoId é obrigatório.");
    }

    [Fact]
    public void Should_Have_Error_When_Quantidade_Is_Invalid()
    {
        // Arrange
        var model = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            Items = new List<PedidoListaItensDto>
                {
                    new PedidoListaItensDto { ProdutoId = Guid.NewGuid(), Quantidade = 0 }
                }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantidade")
            .WithErrorMessage("O campo Quantidade deve ter o valor entre 1 e 9999.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        // Arrange
        var model = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            Items = new List<PedidoListaItensDto>
                {
                    new PedidoListaItensDto { ProdutoId = Guid.NewGuid(), Quantidade = 1 }
                }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PedidoId);
        result.ShouldNotHaveValidationErrorFor(x => x.Items);
        result.ShouldNotHaveValidationErrorFor("Items[0].ProdutoId");
        result.ShouldNotHaveValidationErrorFor("Items[0].Quantidade");
    }

    [Fact]
    public void Should_Have_Error_When_Quantidade_Is_Too_High()
    {
        // Arrange
        var model = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            Items = new List<PedidoListaItensDto>
                {
                    new PedidoListaItensDto { ProdutoId = Guid.NewGuid(), Quantidade = 10000 }
                }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantidade")
            .WithErrorMessage("O campo Quantidade deve ter o valor entre 1 e 9999.");
    }

    [Fact]
    public void Should_Have_Error_When_Quantidade_Is_Negative()
    {
        // Arrange
        var model = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            Items = new List<PedidoListaItensDto>
                {
                    new PedidoListaItensDto { ProdutoId = Guid.NewGuid(), Quantidade = -1 }
                }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantidade")
            .WithErrorMessage("O campo Quantidade deve ter o valor entre 1 e 9999.");
    }
}
