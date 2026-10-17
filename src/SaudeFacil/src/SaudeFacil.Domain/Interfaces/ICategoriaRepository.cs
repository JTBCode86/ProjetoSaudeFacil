using SaudeFacil.Domain.Entities;

namespace SaudeFacil.Domain.Interfaces; // ou SaudeFacil.Application.Interfaces

public interface ICategoriaRepository
{
    Task<List<Categoria>> ObterTodasAsync();
    Task AdicionarAsync(Categoria categoria);
}