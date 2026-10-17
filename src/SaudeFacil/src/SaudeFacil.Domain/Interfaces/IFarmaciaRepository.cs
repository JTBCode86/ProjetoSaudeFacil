using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Domain.Interfaces;

public interface IFarmaciaRepository
{
    Task<Farmacia?> ObterPorIdAsync(Guid id);
    Task<Farmacia?> ObterPorCnpjAsync(string cnpj);
    Task<IEnumerable<Farmacia>> ObterPorBairroAsync(string bairro);
    Task AdicionarAsync(Farmacia farmacia);
    void Atualizar(Farmacia farmacia);
    Task<Farmacia?> ObterPorEmailAsync(string email);

    Task<IEnumerable<Farmacia>> ObterTodasAsync();

}