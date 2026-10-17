using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Infrastructure.Mappings;

public class FarmaciaMapping : IEntityTypeConfiguration<Farmacia>
{
    public void Configure(EntityTypeBuilder<Farmacia> builder)
    {
        builder.ToTable("Farmacias");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.NomeFantasia).IsRequired().HasColumnType("varchar(100)");
        builder.Property(f => f.CNPJ).IsRequired().HasColumnType("varchar(18)");
        builder.Property(f => f.Email).IsRequired().HasColumnType("varchar(100)");
        builder.Property(f => f.SenhaHash).IsRequired().HasColumnType("varchar(255)");
        builder.Property(f => f.Telefone).HasColumnType("varchar(20)");

        // Mapeamento do Value Object "Endereco" na tabela de Farmacias
        builder.OwnsOne(f => f.Endereco, e =>
        {
            e.Property(x => x.CEP).HasColumnName("CEP").HasColumnType("varchar(9)");
            e.Property(x => x.Logradouro).HasColumnName("Logradouro").HasColumnType("varchar(150)");
            e.Property(x => x.Numero).HasColumnName("Numero").HasColumnType("varchar(20)");
            e.Property(x => x.Complemento).HasColumnName("Complemento").HasColumnType("varchar(50)");
            e.Property(x => x.Bairro).HasColumnName("Bairro").HasColumnType("varchar(100)");
            e.Property(x => x.Cidade).HasColumnName("Cidade").HasColumnType("varchar(100)");
            e.Property(x => x.Estado).HasColumnName("Estado").HasColumnType("char(2)");
            e.Property(x => x.Latitude).HasColumnName("Latitude").HasColumnType("float");
            e.Property(x => x.Longitude).HasColumnName("Longitude").HasColumnType("float");
        });

        // Configuração de acesso direto ao campo privado da lista de remédios
        builder.Metadata.FindNavigation(nameof(Farmacia.Remedios))?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}