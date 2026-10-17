namespace SaudeFacil.Application.DTOs;

public class RemedioConsultaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public string? ImagemUrl { get; set; }
    public bool ExigeReceita { get; set; }
    public string NomeFarmacia { get; set; } = string.Empty;
    public string BairroFarmacia { get; set; } = string.Empty;
    public string CategoriaNome { get; set; } = string.Empty;
    public double MediaAvaliacoes { get; set; }
}