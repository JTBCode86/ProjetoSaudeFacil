using Microsoft.EntityFrameworkCore;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Interfaces;
using SaudeFacil.Infrastructure.Data;

namespace SaudeFacil.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly SaudeFacilDbContext _context;

    public PedidoRepository(SaudeFacilDbContext context)
    {
        _context = context;
    }

    public async Task<Pedido?> ObterPorIdAsync(Guid id) =>
        await _context.Pedidos
            .Include(p => p.Itens)
                .ThenInclude(i => i.Remedio)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Pedido>> ObterPorClienteIdAsync(Guid clienteId) =>
        await _context.Pedidos
            .Include(p => p.Itens)
                .ThenInclude(i => i.Remedio)
            .Where(p => p.ClienteId == clienteId)
            .OrderByDescending(p => p.DataPedido)
            .ToListAsync();

    public async Task AdicionarAsync(Pedido pedido) =>
        await _context.Pedidos.AddAsync(pedido);

    public void Atualizar(Pedido pedido) =>
        _context.Pedidos.Update(pedido);
}