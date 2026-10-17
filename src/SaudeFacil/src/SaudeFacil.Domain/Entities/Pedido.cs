namespace SaudeFacil.Domain.Entities;

public class Pedido
{
    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public DateTime DataPedido { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string TipoEntrega { get; private set; } // 'Entrega' ou 'Retirada'
    public string FormaPagamento { get; private set; }
    public string Status { get; private set; } // 'Pendente', 'Concluido', 'Cancelado'

    private readonly List<ItemPedido> _itens = new();
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    private Pedido() { }

    public Pedido(Guid clienteId, string tipoEntrega, string formaPagamento)
    {
        if (clienteId == Guid.Empty) throw new ArgumentException("Cliente inválido.");
        if (string.IsNullOrWhiteSpace(tipoEntrega)) throw new ArgumentException("Tipo de entrega é obrigatório.");
        if (string.IsNullOrWhiteSpace(formaPagamento)) throw new ArgumentException("Forma de pagamento é obrigatória.");

        Id = Guid.NewGuid();
        ClienteId = clienteId;
        TipoEntrega = tipoEntrega;
        FormaPagamento = formaPagamento;
        Status = "Pendente";
        DataPedido = DateTime.UtcNow;
        ValorTotal = 0;
    }

    public void AdicionarItem(Remedio remedio, int quantidade)
    {
        if (remedio == null) throw new ArgumentNullException(nameof(remedio));

        remedio.DeduzirEstoque(quantidade);
        var item = new ItemPedido(remedio.Id, quantidade, remedio.Preco);
        _itens.Add(item);

        ValorTotal += item.SubTotal;
    }

    public void Concluir() => Status = "Concluido";
    public void Cancelar() => Status = "Cancelado";
}