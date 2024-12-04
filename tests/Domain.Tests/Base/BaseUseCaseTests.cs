using Core.Domain.Base;
using Core.Domain.Entities;
using Core.Domain.Notificacoes;
using Domain.Tests.TestHelpers;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.Tests.Base;

public class BaseUseCaseTests
{
    private class TestEntity : Entity
    {
    }

    private class TestValidator : AbstractValidator<TestEntity>
    {
        public TestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id é obrigatório.");
        }
    }

    private class TestUseCase : BaseUseCase
    {
        public TestUseCase(INotificador notificador) : base(notificador)
        {
        }

        public bool TestarValidacao(TestValidator validator, TestEntity entity) =>
            ExecutarValidacao(validator, entity);

        public void TestarNotificacao(ValidationResult validationResult) =>
            Notificar(validationResult);

        public void TestarNotificacao(string mensagem) =>
            Notificar(mensagem);
    }

    [Fact]
    public void Should_Notify_When_Validation_Fails()
    {
        var notificador = new NotificadorFake();
        var useCase = new TestUseCase(notificador);
        var entity = new TestEntity { Id = Guid.Empty };
        var validator = new TestValidator();

        var result = useCase.TestarValidacao(validator, entity);

        Assert.False(result);
        Assert.True(notificador.TemNotificacao());
        Assert.Contains(notificador.ObterNotificacoes(), n => n.Mensagem == "Id é obrigatório.");
    }

    [Fact]
    public void Should_Not_Notify_When_Validation_Succeeds()
    {
        var notificador = new NotificadorFake();
        var useCase = new TestUseCase(notificador);
        var entity = new TestEntity { Id = Guid.NewGuid() };
        var validator = new TestValidator();

        var result = useCase.TestarValidacao(validator, entity);

        Assert.True(result);
        Assert.False(notificador.TemNotificacao());
    }

    [Fact]
    public void Should_Notify_With_ValidationResult()
    {
        var notificador = new NotificadorFake();
        var useCase = new TestUseCase(notificador);
        var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Property", "Erro de validação")
            });

        useCase.TestarNotificacao(validationResult);

        Assert.True(notificador.TemNotificacao());
        Assert.Contains(notificador.ObterNotificacoes(), n => n.Mensagem == "Erro de validação");
    }

    [Fact]
    public void Should_Notify_With_String_Message()
    {
        var notificador = new NotificadorFake();
        var useCase = new TestUseCase(notificador);

        useCase.TestarNotificacao("Mensagem de erro");

        Assert.True(notificador.TemNotificacao());
        Assert.Contains(notificador.ObterNotificacoes(), n => n.Mensagem == "Mensagem de erro");
    }
}