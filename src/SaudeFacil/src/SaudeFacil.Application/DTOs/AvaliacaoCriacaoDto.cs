namespace SaudeFacil.Application.DTOs;

public class AvaliacaoCriacaoDto
{
    public Guid RemedioId { get; set; }
    public Guid ClienteId { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
}