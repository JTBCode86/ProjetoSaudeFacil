using Microsoft.EntityFrameworkCore;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Interfaces;
using SaudeFacil.Infrastructure.Data;

namespace SaudeFacil.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly SaudeFacilDbContext _context;

    public ClienteRepository(SaudeFacilDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente?> ObterPorIdAsync(Guid id) =>
        await _context.Clientes.FindAsync(id);

    public async Task<Cliente?> ObterPorEmailAsync(string email) =>
        await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

    public async Task AdicionarAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync(); // Persiste no banco SQL Server
    }

    public void Atualizar(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        _context.SaveChanges(); // Persiste as alterações no banco
    }
}