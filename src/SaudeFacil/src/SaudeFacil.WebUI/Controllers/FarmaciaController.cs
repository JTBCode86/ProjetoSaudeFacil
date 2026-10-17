using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaudeFacil.Application.DTOs;
using SaudeFacil.Application.Interfaces;

namespace SaudeFacil.WebUI.Controllers;

[Authorize(Roles = "Farmacia")]
public class FarmaciaController : Controller
{
    private readonly IImportacaoService _importacaoService;
    private readonly IFarmaciaService _farmaciaService;

    public FarmaciaController(IImportacaoService importacaoService, IFarmaciaService farmaciaService)
    {
        _importacaoService = importacaoService;
        _farmaciaService = farmaciaService;
    }

    // GET: /Farmacia/ImportarProdutos
    [HttpGet]
    public async Task<IActionResult> ImportarProdutos()
    {
        var farmaciaIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(farmaciaIdClaim) || !Guid.TryParse(farmaciaIdClaim, out var farmaciaId))
        {
            TempData["Erro"] = "Sessão inválida. Por favor, faça login novamente.";
            return RedirectToAction("Login", "Conta");
        }

        // Busca todas e filtra apenas a farmácia logada para o dropdown
        var farmacias = await _farmaciaService.ObterTodasAsync();
        var farmaciaLogada = farmacias.Where(f => f.Id == farmaciaId).ToList();

        ViewBag.Farmacias = new SelectList(farmaciaLogada, "Id", "NomeFantasia", farmaciaId);

        // Retorna o DTO com o ID já pré-preenchido para o input hidden e o select
        return View(new ImportarEstoqueDto { FarmaciaId = farmaciaId });
    }

    // POST: /Farmacia/ImportarProdutos
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportarProdutos(ImportarEstoqueDto dto)
    {
        var farmaciaIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(farmaciaIdClaim) || !Guid.TryParse(farmaciaIdClaim, out var farmaciaId))
        {
            TempData["Erro"] = "Sessão inválida. Por favor, faça login novamente.";
            return RedirectToAction("Login", "Conta");
        }

        // Força o ID da farmácia logada por segurança (evita adulteração no HTML/Request)
        dto.FarmaciaId = farmaciaId;

        // Valida o envio do ficheiro
        if (dto.ArquivoExcel == null || dto.ArquivoExcel.Length == 0)
        {
            ModelState.AddModelError("ArquivoExcel", "Por favor, selecione um arquivo Excel válido.");

            // Recarrega o dropdown filtrado para retornar à View sem quebrar
            var farmacias = await _farmaciaService.ObterTodasAsync();
            ViewBag.Farmacias = new SelectList(farmacias.Where(f => f.Id == farmaciaId), "Id", "NomeFantasia", farmaciaId);
            return View(dto);
        }

        // Executa o serviço de importação associando ao ID da farmácia logada
        var (total, erros) = await _importacaoService.ImportarRemediosExcelAsync(farmaciaId, dto.ArquivoExcel);

        if (erros != null && erros.Any())
        {
            ViewBag.Erros = erros;
        }

        TempData["Sucesso"] = $"{total} produto(s) importado(s) e cadastrado(s) com sucesso!";

        // Recarrega o dropdown filtrado para o retorno da View
        var farmaciasLista = await _farmaciaService.ObterTodasAsync();
        ViewBag.Farmacias = new SelectList(farmaciasLista.Where(f => f.Id == farmaciaId), "Id", "NomeFantasia", farmaciaId);

        return View(dto);
    }
}