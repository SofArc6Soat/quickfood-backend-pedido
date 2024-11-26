using Infra.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItemDb>
    {
        public void Configure(EntityTypeBuilder<PedidoItemDb> builder)
        {
            builder.ToTable("PedidosItens", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ValorUnitario)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);
        }
    }
}
