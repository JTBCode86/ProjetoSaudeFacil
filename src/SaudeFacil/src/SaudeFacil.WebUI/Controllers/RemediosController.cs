using Microsoft.AspNetCore.Mvc;
using SaudeFacil.Application.DTOs;
using SaudeFacil.Application.Interfaces;

namespace SaudeFacil.WebUI.Controllers;

public class RemediosController : Controller
{
    private readonly IRemedioService _remedioService;

    public RemediosController(IRemedioService remedioService)
    {
        _remedioService = remedioService;
    }

    public async Task<IActionResult> Detalhes(Guid id)
    {
        var remedio = await _remedioService.ObterPorIdAsync(id);
        if (remedio == null) return NotFound();

        return View(remedio);
    }

    [HttpPost]
    public async Task<IActionResult> Avaliar(AvaliacaoCriacaoDto dto)
    {
        if (ModelState.IsValid)
        {
            await _remedioService.AvaliarRemedioAsync(dto);
            return RedirectToAction("Detalhes", new { id = dto.RemedioId });
        }
        return RedirectToAction("Detalhes", new { id = dto.RemedioId });
    }
}