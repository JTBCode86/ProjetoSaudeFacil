using SaudeFacil.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaudeFacil.Domain.Entities;

public class Farmacia
{
    public Guid Id { get; private set; }
    public string NomeFantasia { get; private set; }
    public string CNPJ { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; set; }
    public string? Telefone { get; private set; }
    public Endereco Endereco { get; private set; }
    public DateTime DataCadastro { get; private set; }
    [NotMapped]
    public double DistanciaKm { get; set; }

    private readonly List<Remedio> _remedios = new();
    public IReadOnlyCollection<Remedio> Remedios => _remedios.AsReadOnly();

    private Farmacia() { }

    public Farmacia(string nomeFantasia, string cnpj, string email, string senhaHash, string? telefone, Endereco endereco)
    {
        if (string.IsNullOrWhiteSpace(nomeFantasia)) throw new ArgumentException("Nome Fantasia é obrigatório.");
        if (string.IsNullOrWhiteSpace(cnpj) || cnpj.Length < 14) throw new ArgumentException("CNPJ inválido.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) throw new ArgumentException("E-mail inválido.");

        Id = Guid.NewGuid();
        NomeFantasia = nomeFantasia;
        CNPJ = cnpj;
        Email = email;
        SenhaHash = senhaHash;
        Telefone = telefone;
        Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
        DataCadastro = DateTime.UtcNow;
    }
}