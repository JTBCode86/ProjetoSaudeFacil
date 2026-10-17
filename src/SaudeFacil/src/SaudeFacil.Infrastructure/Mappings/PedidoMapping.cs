using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Infrastructure.Mappings;

public class PedidoMapping : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ValorTotal).HasColumnType("decimal(10,2)");
        builder.Property(p => p.TipoEntrega).HasColumnType("varchar(20)");
        builder.Property(p => p.FormaPagamento).HasColumnType("varchar(30)");
        builder.Property(p => p.Status).HasColumnType("varchar(20)");

        builder.HasMany(p => p.Itens)
               .WithOne()
               .HasForeignKey(i => i.PedidoId);

        builder.Metadata.FindNavigation(nameof(Pedido.Itens))?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}