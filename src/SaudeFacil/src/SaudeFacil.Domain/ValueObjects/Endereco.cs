namespace SaudeFacil.Domain.ValueObjects;

public class Endereco
{
    public string CEP { get; private set; }
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string? Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    // Construtor privado exigido pelo EF Core
    private Endereco() { }

    public Endereco(string cep, string logradouro, string numero, string? complemento, string bairro, string cidade, string estado, double latitude = 0, double longitude = 0)
    {
        if (string.IsNullOrWhiteSpace(cep)) throw new ArgumentException("CEP é obrigatório.");
        if (string.IsNullOrWhiteSpace(logradouro)) throw new ArgumentException("Logradouro é obrigatório.");
        if (string.IsNullOrWhiteSpace(numero)) throw new ArgumentException("Número é obrigatório.");
        if (string.IsNullOrWhiteSpace(bairro)) throw new ArgumentException("Bairro é obrigatório.");
        if (string.IsNullOrWhiteSpace(cidade)) throw new ArgumentException("Cidade é obrigatória.");
        if (string.IsNullOrWhiteSpace(estado) || estado.Length != 2) throw new ArgumentException("Estado deve ter 2 caracteres.");

        CEP = cep;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado.ToUpper();
        Latitude = latitude;
        Longitude = longitude;
    }
}