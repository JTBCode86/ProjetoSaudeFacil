namespace SaudeFacil.Domain.Entities;

public class Remedio
{
    public Guid Id { get; private set; }
    public Guid FarmaciaId { get; private set; }
    public int CategoriaId { get; private set; }
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public int Estoque { get; private set; }
    public string? ImagemUrl { get; private set; }
    public bool ExigeReceita { get; private set; }
    public bool Ativo { get; private set; }

    // Propriedades de navegação
    public Farmacia Farmacia { get; private set; }
    public Categoria Categoria { get; private set; }

    private readonly List<Avaliacao> _avaliacoes = new();
    public IReadOnlyCollection<Avaliacao> Avaliacoes => _avaliacoes.AsReadOnly();

    private Remedio() { }

    public Remedio(Guid farmaciaId, int categoriaId, string nome, string? descricao, decimal preco, int estoque, string? imagemUrl, bool exigeReceita)
    {
        if (farmaciaId == Guid.Empty) throw new ArgumentException("Farmácia inválida.");
        if (categoriaId <= 0) throw new ArgumentException("Categoria inválida.");
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome do remédio é obrigatório.");
        if (preco <= 0) throw new ArgumentException("Preço deve ser maior que zero.");
        if (estoque < 0) throw new ArgumentException("Estoque não pode ser negativo.");

        Id = Guid.NewGuid();
        FarmaciaId = farmaciaId;
        CategoriaId = categoriaId;
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Estoque = estoque;
        ImagemUrl = imagemUrl;
        ExigeReceita = exigeReceita;
        Ativo = true;
    }

    public void DeduzirEstoque(int quantidade)
    {
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser maior que zero.");
        if (quantidade > Estoque) throw new InvalidOperationException("Estoque insuficiente.");
        Estoque -= quantidade;
    }

    public void AdicionarEstoque(int quantidade)
    {
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser maior que zero.");
        Estoque += quantidade;
    }
}