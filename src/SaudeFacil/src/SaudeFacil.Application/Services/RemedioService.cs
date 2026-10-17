using SaudeFacil.Application.DTOs;
using SaudeFacil.Application.Interfaces;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Exceptions;
using SaudeFacil.Domain.Interfaces;

namespace SaudeFacil.Application.Services;

public class RemedioService : IRemedioService
{
    private readonly IRemedioRepository _remedioRepository;

    public RemedioService(IRemedioRepository remedioRepository)
    {
        _remedioRepository = remedioRepository;
    }

    public async Task<IEnumerable<RemedioConsultaDto>> BuscarRemediosAsync(string? termo, int? categoriaId)
    {
        var remedios = await _remedioRepository.BuscarPorNomeOuCategoriaAsync(termo, categoriaId);

        return remedios.Select(r => new RemedioConsultaDto
        {
            Id = r.Id,
            Nome = r.Nome,
            Descricao = r.Descricao,
            Preco = r.Preco,
            Estoque = r.Estoque,
            ImagemUrl = r.ImagemUrl,
            ExigeReceita = r.ExigeReceita,
            NomeFarmacia = r.Farmacia?.NomeFantasia ?? string.Empty,
            BairroFarmacia = r.Farmacia?.Endereco?.Bairro ?? string.Empty,
            CategoriaNome = r.Categoria?.Nome ?? string.Empty,
            MediaAvaliacoes = CalcularMediaAvaliacao(r) // r.Avaliacoes.Any() ? r.Avaliacoes.Average(a => a.Nota) : 4.5
        });
    }

    public async Task<RemedioConsultaDto?> ObterPorIdAsync(Guid id)
    {
        var r = await _remedioRepository.ObterPorIdAsync(id);
        if (r == null) return null;

        return new RemedioConsultaDto
        {
            Id = r.Id,
            Nome = r.Nome,
            Descricao = r.Descricao,
            Preco = r.Preco,
            Estoque = r.Estoque,
            ImagemUrl = r.ImagemUrl,
            ExigeReceita = r.ExigeReceita,
            NomeFarmacia = r.Farmacia?.NomeFantasia ?? string.Empty,
            BairroFarmacia = r.Farmacia?.Endereco?.Bairro ?? string.Empty,
            CategoriaNome = r.Categoria?.Nome ?? string.Empty,
            MediaAvaliacoes = CalcularMediaAvaliacao(r) //r.Avaliacoes.Any() ? r.Avaliacoes.Average(a => a.Nota) : 4.5
        };
    }

    public async Task AvaliarRemedioAsync(AvaliacaoCriacaoDto dto)
    {
        var remedio = await _remedioRepository.ObterPorIdAsync(dto.RemedioId);
        if (remedio == null)
            throw new DomainException("Remédio não encontrado.");

        var avaliacao = new Avaliacao(dto.RemedioId, dto.ClienteId, dto.Nota, dto.Comentario);
        await _remedioRepository.AdicionarAvaliacaoAsync(avaliacao);
    }

    public async Task AdicionarEmLoteAsync(IEnumerable<Remedio> remedios)
    {
        await _remedioRepository.AdicionarEmLoteAsync(remedios);
    }

    // Método auxiliar para calcular a média real ou gerar uma nota dinâmica baseada no ID
    private double CalcularMediaAvaliacao(Remedio r)
    {
        if (r.Avaliacoes != null && r.Avaliacoes.Any())
        {
            return Math.Round(r.Avaliacoes.Average(a => a.Nota), 1);
        }

        // Gera uma nota dinâmica e fixa por produto entre 4.0 e 5.0 usando o Hash do ID
        int hash = Math.Abs(r.Id.GetHashCode());
        double notaFicticia = 4.0 + (hash % 11) * 0.1; // Ex: 4.0, 4.1, 4.2 ... 5.0
        return Math.Round(notaFicticia, 1);
    }
}