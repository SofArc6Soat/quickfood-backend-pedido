using Core.Domain.Validacoes;
using Xunit;

namespace Core.Domain.Tests.Validacoes;

public class ValidadorCpfTests
{
    [Theory]
    [InlineData("111.111.111-11", false)] // CPF com dígitos repetidos
    [InlineData("529.982.247-25", true)]  // CPF válido
    [InlineData("52998224725", true)]     // CPF válido sem caracteres especiais
    [InlineData("529.982.247-2", false)]  // CPF com tamanho inválido
    [InlineData("000.000.000-00", false)] // CPF com dígitos repetidos
    [InlineData("222.222.222-22", false)] // CPF com dígitos repetidos
    [InlineData("333.333.333-33", false)] // CPF com dígitos repetidos
    [InlineData("444.444.444-44", false)] // CPF com dígitos repetidos
    [InlineData("555.555.555-55", false)] // CPF com dígitos repetidos
    [InlineData("666.666.666-66", false)] // CPF com dígitos repetidos
    [InlineData("777.777.777-77", false)] // CPF com dígitos repetidos
    [InlineData("888.888.888-88", false)] // CPF com dígitos repetidos
    [InlineData("999.999.999-99", false)] // CPF com dígitos repetidos
    public void Validar_Should_ReturnExpectedResult(string cpf, bool expectedResult)
    {
        // Act
        var result = ValidadorCpf.Validar(cpf);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
