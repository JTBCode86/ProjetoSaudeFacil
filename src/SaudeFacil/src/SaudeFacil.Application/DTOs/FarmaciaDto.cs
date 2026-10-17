namespace SaudeFacil.Application.DTOs;

public class FarmaciaDto
{
    public Guid Id { get; set; } // Ajustado para Guid
    public string NomeFantasia { get; set; } = string.Empty;

    public string SenhaHash { get; set; }

    public string Email { get; set; }

    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double DistanciaKm { get; set; }
}