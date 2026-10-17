namespace SaudeFacil.Domain.Entities;

public class Avaliacao
{
    public Guid Id { get; private set; }
    public Guid RemedioId { get; private set; }
    public Guid ClienteId { get; private set; }
    public int Nota { get; private set; }
    public string? Comentario { get; private set; }
    public DateTime DataAvaliacao { get; private set; }

    private Avaliacao() { }

    public Avaliacao(Guid remedioId, Guid clienteId, int nota, string? comentario)
    {
        if (nota < 1 || nota > 5) throw new ArgumentOutOfRangeException(nameof(nota), "A nota deve ser entre 1 e 5.");

        Id = Guid.NewGuid();
        RemedioId = remedioId;
        ClienteId = clienteId;
        Nota = nota;
        Comentario = comentario;
        DataAvaliacao = DateTime.UtcNow;
    }
}