using Microsoft.EntityFrameworkCore;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Interfaces;
using SaudeFacil.Infrastructure.Data;

namespace SaudeFacil.Infrastructure.Repositories;

public class FarmaciaRepository : IFarmaciaRepository
{
    private readonly SaudeFacilDbContext _context;

    public FarmaciaRepository(SaudeFacilDbContext context)
    {
        _context = context;
    }

    public async Task<Farmacia?> ObterPorIdAsync(Guid id) =>
        await _context.Farmacias
            .Include(f => f.Remedios)
            .FirstOrDefaultAsync(f => f.Id == id);

    public async Task<Farmacia?> ObterPorCnpjAsync(string cnpj) =>
        await _context.Farmacias
            .FirstOrDefaultAsync(f => f.CNPJ == cnpj);

    public async Task<IEnumerable<Farmacia>> ObterPorBairroAsync(string bairro) =>
        await _context.Farmacias
            .Where(f => f.Endereco.Bairro.Contains(bairro))
            .ToListAsync();

    public async Task AdicionarAsync(Farmacia farmacia)
    {
        await _context.Farmacias.AddAsync(farmacia);
        await _context.SaveChangesAsync(); 
    }


    public void Atualizar(Farmacia farmacia) =>
        _context.Farmacias.Update(farmacia);

    public async Task<Farmacia?> ObterPorEmailAsync(string email)
    {
         return await _context.Farmacias.FirstOrDefaultAsync(f => f.Email == email);
    }

    public async Task<IEnumerable<Farmacia>> ObterTodasAsync()
    {
        return await _context.Farmacias
        .Include(f => f.Endereco)
        .ToListAsync();
    }
}