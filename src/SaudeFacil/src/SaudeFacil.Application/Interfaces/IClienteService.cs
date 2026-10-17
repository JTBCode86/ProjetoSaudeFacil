using SaudeFacil.Application.DTOs;
using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Application.Interfaces;

public interface IClienteService
{
    Task<Guid> CadastrarAsync(CadastrarClienteDto dto);
    Task<Cliente?> AutenticarAsync(string email, string senha);
}