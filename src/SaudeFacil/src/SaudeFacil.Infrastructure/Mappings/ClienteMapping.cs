using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Infrastructure.Mappings;

public class ClienteMapping : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome).IsRequired().HasColumnType("varchar(100)");
        builder.Property(c => c.Email).IsRequired().HasColumnType("varchar(100)");
        builder.Property(c => c.SenhaHash).IsRequired().HasColumnType("varchar(255)");
        builder.Property(c => c.Telefone).HasColumnType("varchar(20)");

        // Mapeamento do Value Object "Endereco" para a mesma tabela
        builder.OwnsOne(c => c.Endereco, e =>
        {
            e.Property(x => x.CEP).HasColumnName("CEP").HasColumnType("varchar(9)");
            e.Property(x => x.Logradouro).HasColumnName("Logradouro").HasColumnType("varchar(150)");
            e.Property(x => x.Numero).HasColumnName("Numero").HasColumnType("varchar(20)");
            e.Property(x => x.Complemento).HasColumnName("Complemento").HasColumnType("varchar(50)");
            e.Property(x => x.Bairro).HasColumnName("Bairro").HasColumnType("varchar(100)");
            e.Property(x => x.Cidade).HasColumnName("Cidade").HasColumnType("varchar(100)");
            e.Property(x => x.Estado).HasColumnName("Estado").HasColumnType("char(2)");
        });

        // Configuração para o EF Core preencher a lista privada
        builder.Metadata.FindNavigation(nameof(Cliente.Pedidos))?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}