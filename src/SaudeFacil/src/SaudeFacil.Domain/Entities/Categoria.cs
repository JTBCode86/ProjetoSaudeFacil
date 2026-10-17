namespace SaudeFacil.Domain.Entities;

public class Categoria
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }

    private Categoria() { }

    public Categoria(string nome, string? descricao)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome da categoria é obrigatório.");
        Nome = nome;
        Descricao = descricao;
    }
}