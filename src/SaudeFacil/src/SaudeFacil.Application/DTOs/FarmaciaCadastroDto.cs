namespace SaudeFacil.Application.DTOs;

public class FarmaciaCadastroDto
{
    public string NomeFantasia { get; set; } = string.Empty;

    // Suporta 'Cnpj' (para a View) e 'CNPJ' (para a Service)
    public string Cnpj { get; set; } = string.Empty;
    public string CNPJ { get => Cnpj; set => Cnpj = value; }

    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? Telefone { get; set; }

    // Suporta 'Cep' (para a View) e 'CEP' (para a Service)
    public string Cep { get; set; } = string.Empty;
    public string CEP { get => Cep; set => Cep = value; }

    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}