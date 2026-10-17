using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using SaudeFacil.Application.Interfaces;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Interfaces; // Namespace dos repositórios

namespace SaudeFacil.Application.Services;

public class ImportacaoService : IImportacaoService
{
    private readonly IRemedioRepository _remedioRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public ImportacaoService(IRemedioRepository remedioRepository, ICategoriaRepository categoriaRepository)
    {
        _remedioRepository = remedioRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<(int totalProcessados, List<string> erros)> ImportarRemediosExcelAsync(Guid farmaciaId, IFormFile arquivo)
    {
        var erros = new List<string>();
        int totalProcessados = 0;

        if (arquivo == null || arquivo.Length == 0)
        {
            erros.Add("O arquivo enviado está vazio.");
            return (0, erros);
        }

        var categorias = await _categoriaRepository.ObterTodasAsync();

        using var stream = arquivo.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RowsUsed().Skip(1); // Pula cabeçalho

        int linhaAtual = 2;
        var novosRemedios = new List<Remedio>();

        foreach (var row in rows)
        {
            try
            {
                string nome = row.Cell(1).GetValue<string>().Trim();
                string categoriaNome = row.Cell(2).GetValue<string>().Trim();
                string descricao = row.Cell(3).GetValue<string>().Trim();
                decimal preco = row.Cell(4).GetValue<decimal>();
                int estoque = row.Cell(5).GetValue<int>();
                string exigeReceitaStr = row.Cell(6).GetValue<string>().Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(nome))
                {
                    erros.Add($"Linha {linhaAtual}: Nome do medicamento é obrigatório.");
                    linhaAtual++;
                    continue;
                }

                var categoria = categorias.FirstOrDefault(c => c.Nome.Equals(categoriaNome, StringComparison.OrdinalIgnoreCase));
                if (categoria == null)
                {
                    categoria = new Categoria(categoriaNome, "Categoria gerada via importação");
                    await _categoriaRepository.AdicionarAsync(categoria);
                    categorias.Add(categoria);
                }

                bool exigeReceita = exigeReceitaStr == "SIM" || exigeReceitaStr == "TRUE" || exigeReceitaStr == "1";

                var remedio = new Remedio(
                    farmaciaId,
                    categoria.Id,
                    nome,
                    descricao,
                    preco,
                    estoque,
                    imagemUrl: null,
                    exigeReceita: exigeReceita
                );

                novosRemedios.Add(remedio);
                totalProcessados++;
            }
            catch (Exception ex)
            {
                erros.Add($"Linha {linhaAtual}: Erro ao ler dados ({ex.Message}).");
            }

            linhaAtual++;
        }

        if (novosRemedios.Any())
        {
            await _remedioRepository.AdicionarEmLoteAsync(novosRemedios);
        }

        return (totalProcessados, erros);
    }
}