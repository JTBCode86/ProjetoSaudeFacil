using SaudeFacil.Application.DTOs;
using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Application.Interfaces;

public interface IFarmaciaService
{
    Task<Guid> CadastrarAsync(FarmaciaCadastroDto dto);
    Task<Farmacia?> AutenticarAsync(string email, string senha);

    Task<IEnumerable<FarmaciaDto>> ObterTodasAsync();
}