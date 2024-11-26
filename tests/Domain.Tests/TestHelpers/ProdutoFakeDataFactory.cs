using Domain.ValueObjects;
using Infra.Dto;

namespace Domain.Tests.TestHelpers;

public static class ProdutoFakeDataFactory
{
    public static ProdutoDb CriarProdutoDbValido() => new()
    {
        Id = ObterGuid(),
        Nome = "Produto Exemplo",
        Descricao = "Descrição do Produto",
        Preco = 100.00m,
        Categoria = Categoria.Lanche.ToString(),
        Ativo = true
    };

    public static ProdutoDb CriarProdutoDbValido2() => new()
    {
        Id = ObterGuid2(),
        Nome = "Produto Exemplo",
        Descricao = "Descrição do Produto",
        Preco = 100.00m,
        Categoria = Categoria.Lanche.ToString(),
        Ativo = true
    };

    public static ProdutoDb CriarProdutoDbInvalido() => new()
    {
        Id = ObterGuid3(),
        Nome = string.Empty, // Nome inválido
        Descricao = string.Empty, // Descrição inválida
        Preco = -10.00m, // Preço inválido
        Categoria = string.Empty, // Categoria inválida
        Ativo = false
    };

    public static Guid ObterGuid() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");

    public static Guid ObterGuid2() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0852");

    public static Guid ObterGuid3() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0853");
}