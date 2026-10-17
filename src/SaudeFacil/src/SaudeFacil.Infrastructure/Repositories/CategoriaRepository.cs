using Microsoft.EntityFrameworkCore;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Interfaces;
using SaudeFacil.Infrastructure.Data;

namespace SaudeFacil.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly SaudeFacilDbContext _context;

    public CategoriaRepository(SaudeFacilDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> ObterTodasAsync()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria?> ObterPorIdAsync(int id)
    {
        return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AdicionarAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var categoria = await ObterPorIdAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}