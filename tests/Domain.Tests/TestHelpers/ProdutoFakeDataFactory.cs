using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;

namespace Domain.Tests.TestHelpers;

public static class ProdutoFakeDataFactory
{
    public static Produto CriarProdutoValido() => new(ObterGuid(), "Produto Exemplo", "Descrição do Produto", 100.00m, "Lanche", true);

    public static Produto AlterarProdutoValido() => new(ObterGuid(), "Produto Exemplo 2", "Descrição do Produto 3", 120.00m, "Lanche", true);

    public static Produto CriarProdutoInvalido() => new(ObterGuid(), "A", "A", 00.00m, "Lanche", true);

    public static ProdutoDb CriarProdutoDbValido() => new()
    {
        Id = ObterGuid(),
        Nome = "Produto Exemplo",
        Descricao = "Descrição do Produto",
        Preco = 100.00m,
        Categoria = Categoria.Lanche.ToString(),
        Ativo = true
    };

    public static Guid ObterGuid() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");
}