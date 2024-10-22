using Infra.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class PedidoMapping : IEntityTypeConfiguration<PedidoDb>
    {
        public void Configure(EntityTypeBuilder<PedidoDb> builder)
        {
            builder.ToTable("Pedidos", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ValorTotal)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);

            builder.Property(c => c.Status)
                   .IsRequired()
                   .HasColumnType("varchar(20)");

            // Identity
            builder.Property(c => c.NumeroPedido).UseIdentityColumn(10, 1).ValueGeneratedOnAddOrUpdate();

            // EF Rel.
            builder.HasMany(e => e.Itens)
                .WithOne(e => e.Pedido)
                .HasForeignKey(e => e.PedidoId);
        }
    }
}
