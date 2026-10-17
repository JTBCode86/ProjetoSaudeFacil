using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SaudeFacil.Application.DTOs;

public class ImportarEstoqueDto
{
    [Required(ErrorMessage = "Selecione a farmácia.")]
    public Guid FarmaciaId { get; set; }

    [Required(ErrorMessage = "Por favor, selecione um arquivo Excel (.xlsx).")]
    public IFormFile ArquivoExcel { get; set; } = default!;
}