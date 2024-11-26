using Infra.Dto;
using Infra.Mappings.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class ProdutoMapping : IEntityTypeConfiguration<ProdutoDb>
    {
        public void Configure(EntityTypeBuilder<ProdutoDb> builder)
        {
            builder.ToTable("Produtos", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                   .IsRequired()
                   .HasColumnType("varchar(40)");

            builder.Property(c => c.Descricao)
                   .IsRequired()
                   .HasColumnType("varchar(200)");

            builder.Property(c => c.Categoria)
                   .IsRequired()
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.Preco)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);

            // Data
            builder.HasData(ProdutoSeedData.GetSeedData());
        }
    }
}