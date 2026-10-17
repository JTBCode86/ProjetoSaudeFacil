using Microsoft.AspNetCore.Http;

namespace SaudeFacil.Application.Interfaces;

public interface IImportacaoService
{
    Task<(int totalProcessados, List<string> erros)> ImportarRemediosExcelAsync(Guid farmaciaId, IFormFile arquivo);
}