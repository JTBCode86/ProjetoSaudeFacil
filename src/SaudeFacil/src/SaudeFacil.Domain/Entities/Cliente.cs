using SaudeFacil.Domain.ValueObjects;

namespace SaudeFacil.Domain.Entities;

public class Cliente
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public string? Telefone { get; private set; }
    public Endereco Endereco { get; private set; }
    public DateTime DataCadastro { get; private set; }

    private readonly List<Pedido> _pedidos = new();
    public IReadOnlyCollection<Pedido> Pedidos => _pedidos.AsReadOnly();

    private Cliente() { }

    public Cliente(string nome, string cpf, string email, string senhaHash, string? telefone, Endereco endereco)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(cpf)) throw new ArgumentException("CPF é obrigatório.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) throw new ArgumentException("E-mail inválido.");
        if (string.IsNullOrWhiteSpace(senhaHash)) throw new ArgumentException("Senha é obrigatória.");

        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Email = email;
        SenhaHash = senhaHash;
        Telefone = telefone;
        Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
        DataCadastro = DateTime.UtcNow;
    }

    public void AtualizarEndereco(Endereco novoEndereco)
    {
        Endereco = novoEndereco ?? throw new ArgumentNullException(nameof(novoEndereco));
    }
}