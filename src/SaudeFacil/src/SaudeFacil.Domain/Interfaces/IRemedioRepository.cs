using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Domain.Interfaces;

public interface IRemedioRepository
{
    Task<Remedio?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Remedio>> ObterTodosAsync();
    Task<IEnumerable<Remedio>> BuscarPorNomeOuCategoriaAsync(string? termo, int? categoriaId);
    Task<IEnumerable<Remedio>> ObterPorFarmaciaIdAsync(Guid farmaciaId);
    Task AdicionarAsync(Remedio remedio);
    void Atualizar(Remedio remedio);
    Task AdicionarAvaliacaoAsync(Avaliacao avaliacao);

    Task AdicionarEmLoteAsync(IEnumerable<Remedio> remedios);

}