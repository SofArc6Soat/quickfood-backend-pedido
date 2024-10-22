using Infra.Dto;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class ProdutoSeedData
    {
        public static List<ProdutoDb> GetSeedData() =>
        [
            #region Lanches
            new ProdutoDb {
                Id = Guid.Parse("efee2d79-ce89-479a-9667-04f57f9e2e5e"),
                Nome = "X-SALADA",
                Descricao = "Pão brioche, hambúrguer (150g), queijo prato, pepino, tomate italiano e alface americana.",
                Preco = 30,
                Categoria = "Lanche",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("fdff63d2-127f-49c5-854a-e47cae8cedb9"),
                Nome = "X-BACON",
                Descricao = "Pão brioche, hambúrguer (150g), queijo prato, bacon, pepino, tomate italiano e alface americana.",
                Preco = 33,
                Categoria = "Lanche",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("eee57eb1-1dde-4162-998f-d7148d0c2417"),
                Nome = "X-BURGUER",
                Descricao = "Pão brioche, hambúrguer (150g) e queijo prato.",
                Preco = 28,
                Categoria = "Lanche",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("719bc73f-b90a-4bb0-b6d0-8060ea9f1d4c"),
                Nome = "X-DUPLO BACON",
                Descricao = "Pão smash, 2 hambúrgueres (150g cada), maionese do feio ,2 queijos cheddar e muito bacon.",
                Preco = 36,
                Categoria = "Lanche",
                Ativo = true
            },
            #endregion

            #region Acompanhamentos
            new ProdutoDb {
                Id = Guid.Parse("50ba333a-c804-4d2a-a284-9ff1d147df4e"),
                Nome = "BATATA FRITA",
                Descricao = "Porção individual de batata frita (100g)",
                Preco = 9,
                Categoria = "Acompanhamento",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("1bb2aef8-97d7-4fb0-94f5-53bff2f3a618"),
                Nome = "ONION RINGS",
                Descricao = "Anéis de cebola (100g)",
                Preco = 10,
                Categoria = "Acompanhamento",
                Ativo = true
            },
            #endregion

            #region Bebidas
            new ProdutoDb {
                Id = Guid.Parse("111cb598-2df6-41bf-b51b-d4e0f292bda3"),
                Nome = "PEPSI LATA",
                Descricao = "350ml",
                Preco = 7,
                Categoria = "Bebida",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("c0eab3dc-2ddf-4dde-a64f-392f2412201f"),
                Nome = "GUARANÁ ANTARCTICA LATA",
                Descricao = "350ml",
                Preco = 7,
                Categoria = "Bebida",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("3de0c5e7-787b-4885-8ec8-020866971d3b"),
                Nome = "ÁGUA",
                Descricao = "500ml",
                Preco = 5,
                Categoria = "Bebida",
                Ativo = true
            },
            #endregion

            #region Sobremesas
            new ProdutoDb {
                Id = Guid.Parse("b17f425e-e0ff-41cd-92a6-00d78172d7a5"),
                Nome = "BROWNIE CHOCOLATE",
                Descricao = "70g",
                Preco = 10,
                Categoria = "Sobremesa",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("e206c795-d6d6-491e-90ed-fdc08e057939"),
                Nome = "BROWNIE CHOCOLATE BRANCO",
                Descricao = "70g",
                Preco = 10,
                Categoria = "Sobremesa",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("c398d290-d1a1-4f2e-a907-ef55e92beef6"),
                Nome = "SORVETE DE CHOCOLATE",
                Descricao = "100g",
                Preco = 12,
                Categoria = "Sobremesa",
                Ativo = true
            },

            new ProdutoDb {
                Id = Guid.Parse("782725ea-70a5-49db-95b2-c4eea841ebca"),
                Nome = "SORVETE DE CREME",
                Descricao = "100g",
                Preco = 12,
                Categoria = "Sobremesa",
                Ativo = true
            },
            #endregion
        ];
    }
}