using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Infrastructure.Mappings;

public class RemedioMapping : IEntityTypeConfiguration<Remedio>
{
    public void Configure(EntityTypeBuilder<Remedio> builder)
    {
        builder.ToTable("Remedios");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Nome).IsRequired().HasColumnType("varchar(100)");
        builder.Property(r => r.Descricao).HasColumnType("varchar(max)");
        builder.Property(r => r.Preco).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(r => r.ImagemUrl).HasColumnType("varchar(500)");

        builder.HasOne(r => r.Farmacia)
               .WithMany(f => f.Remedios)
               .HasForeignKey(r => r.FarmaciaId);

        builder.HasOne(r => r.Categoria)
               .WithMany()
               .HasForeignKey(r => r.CategoriaId);
    }
}