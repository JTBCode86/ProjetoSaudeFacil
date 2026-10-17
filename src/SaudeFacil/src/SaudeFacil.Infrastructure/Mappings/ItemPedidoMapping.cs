using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Infrastructure.Mappings
{
    public class ItemPedidoMapping : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.ToTable("ItemPedido");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.PrecoUnitario).HasColumnType("decimal(10,2)");

            builder.HasOne(i => i.Remedio)
                   .WithMany()
                   .HasForeignKey(i => i.RemedioId);
        }
    }
}
