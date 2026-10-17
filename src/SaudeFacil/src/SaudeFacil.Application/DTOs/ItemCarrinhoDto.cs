namespace SaudeFacil.Application.DTOs;

public class ItemCarrinhoDto
{
    public Guid RemedioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string FarmaciaNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}