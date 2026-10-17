using Microsoft.EntityFrameworkCore;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Interfaces;
using SaudeFacil.Infrastructure.Data;

namespace SaudeFacil.Infrastructure.Repositories;

public class RemedioRepository : IRemedioRepository
{
    private readonly SaudeFacilDbContext _context;

    public RemedioRepository(SaudeFacilDbContext context)
    {
        _context = context;
    }

    public async Task<Remedio?> ObterPorIdAsync(Guid id) =>
        await _context.Remedios
            .Include(r => r.Categoria)
            .Include(r => r.Farmacia)
                .ThenInclude(f => f.Endereco)
            .Include(r => r.Avaliacoes)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Remedio>> ObterTodosAsync() =>
        await _context.Remedios
            .Include(r => r.Avaliacoes)
            .Where(r => r.Ativo)
            .ToListAsync();

    public async Task<IEnumerable<Remedio>> BuscarPorNomeOuCategoriaAsync(string? termo, int? categoriaId)
    {
        var query = _context.Remedios
            .Include(r => r.Categoria)
            .Include(r => r.Farmacia)
                .ThenInclude(f => f.Endereco)
            .Include(r => r.Avaliacoes)
            .Where(r => r.Ativo)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(termo))
            query = query.Where(r => r.Nome.Contains(termo) || r.Descricao.Contains(termo));

        if (categoriaId.HasValue && categoriaId > 0)
            query = query.Where(r => r.CategoriaId == categoriaId.Value);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Remedio>> ObterPorFarmaciaIdAsync(Guid farmaciaId) =>
        await _context.Remedios
            .Include(r => r.Avaliacoes)
            .Where(r => r.FarmaciaId == farmaciaId && r.Ativo)
            .ToListAsync();

    public async Task AdicionarAsync(Remedio remedio) => await _context.Remedios.AddAsync(remedio);

    public void Atualizar(Remedio remedio) => _context.Remedios.Update(remedio);

    public async Task AdicionarAvaliacaoAsync(Avaliacao avaliacao)
    {
        await _context.Avaliacoes.AddAsync(avaliacao);
        await _context.SaveChangesAsync();
    }

    public async Task AdicionarEmLoteAsync(IEnumerable<Remedio> remedios)
    {
        await _context.Remedios.AddRangeAsync(remedios);
        await _context.SaveChangesAsync();
    }
}