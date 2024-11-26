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

    public static Guid ObterGuid() => Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");
}