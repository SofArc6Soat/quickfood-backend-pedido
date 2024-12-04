using Core.Domain.Entities;

namespace Domain.Tests.Exception;

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_DeveSerCriadaSemParametros()
    {
        // Act
        var exception = new DomainException();

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("Exception of type 'Core.Domain.Entities.DomainException' was thrown.", exception.Message);
    }

    [Fact]
    public void DomainException_DeveSerCriadaComMensagem()
    {
        // Arrange
        var mensagem = "Mensagem de erro";

        // Act
        var exception = new DomainException(mensagem);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagem, exception.Message);
    }

    [Fact]
    public void DomainException_DeveSerCriadaComMensagemEInnerException()
    {
        // Arrange
        var mensagem = "Mensagem de erro";
        var innerException = new DomainException("Inner exception");

        // Act
        var exception = new DomainException(mensagem, innerException);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(mensagem, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}