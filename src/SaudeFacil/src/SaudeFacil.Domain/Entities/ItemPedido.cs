namespace SaudeFacil.Domain.Entities;

public class ItemPedido
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public Guid RemedioId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public decimal SubTotal => Quantidade * PrecoUnitario;

    public Remedio Remedio { get; private set; }

    private ItemPedido() { }

    public ItemPedido(Guid remedioId, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser maior que zero.");
        if (precoUnitario <= 0) throw new ArgumentException("Preço unitário deve ser maior que zero.");

        Id = Guid.NewGuid();
        RemedioId = remedioId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}