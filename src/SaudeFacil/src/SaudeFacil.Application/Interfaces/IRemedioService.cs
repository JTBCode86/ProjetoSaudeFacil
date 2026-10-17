using SaudeFacil.Application.DTOs;
using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Application.Interfaces;

public interface IRemedioService
{
    Task<IEnumerable<RemedioConsultaDto>> BuscarRemediosAsync(string? termo, int? categoriaId);
    Task<RemedioConsultaDto?> ObterPorIdAsync(Guid id);
    Task AvaliarRemedioAsync(AvaliacaoCriacaoDto dto);

    Task AdicionarEmLoteAsync(IEnumerable<Remedio> remedios);
}