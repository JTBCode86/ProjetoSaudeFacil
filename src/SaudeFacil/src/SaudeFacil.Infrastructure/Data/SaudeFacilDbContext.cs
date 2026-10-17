using Microsoft.EntityFrameworkCore;
using SaudeFacil.Domain.Entities;
using System.Reflection;

namespace SaudeFacil.Infrastructure.Data;

public class SaudeFacilDbContext : DbContext
{
    public SaudeFacilDbContext(DbContextOptions<SaudeFacilDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Farmacia> Farmacias { get; set; }
    public DbSet<Remedio> Remedios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas as configurações de mapeamento (Fluent API) que criaremos abaixo
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}