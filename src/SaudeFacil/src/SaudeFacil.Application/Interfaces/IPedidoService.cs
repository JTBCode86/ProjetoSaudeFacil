using SaudeFacil.Application.DTOs;

namespace SaudeFacil.Application.Interfaces;

public interface IPedidoService
{
    Task<Guid> CriarPedidoAsync(PedidoCriacaoDto dto);
}