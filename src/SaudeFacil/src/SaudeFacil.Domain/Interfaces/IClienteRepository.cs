using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Domain.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(Guid id);
    Task<Cliente?> ObterPorEmailAsync(string email);
    Task AdicionarAsync(Cliente cliente);
    void Atualizar(Cliente cliente);

}