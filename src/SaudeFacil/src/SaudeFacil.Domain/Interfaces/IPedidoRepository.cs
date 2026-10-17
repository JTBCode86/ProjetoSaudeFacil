using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Pedido>> ObterPorClienteIdAsync(Guid clienteId);
    Task AdicionarAsync(Pedido pedido);
    void Atualizar(Pedido pedido);
}