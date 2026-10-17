namespace SaudeFacil.Application.DTOs;

public class PedidoCriacaoDto
{
    public Guid ClienteId { get; set; }
    public string TipoEntrega { get; set; } = string.Empty;
    public string FormaPagamento { get; set; } = string.Empty;
    public List<ItemCarrinhoDto> Itens { get; set; } = new();
}