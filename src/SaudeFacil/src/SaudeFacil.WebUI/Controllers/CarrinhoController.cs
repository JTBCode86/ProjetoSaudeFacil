using Microsoft.AspNetCore.Mvc;
using SaudeFacil.Application.DTOs;
using SaudeFacil.Application.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace SaudeFacil.WebUI.Controllers;

public class CarrinhoController : Controller
{
    private readonly IPedidoService _pedidoService;
    private readonly IRemedioService _remedioService;
    private const string CARRINHO_KEY = "CarrinhoSaudeFacil";

    public CarrinhoController(IPedidoService pedidoService, IRemedioService remedioService)
    {
        _pedidoService = pedidoService;
        _remedioService = remedioService;
    }

    public IActionResult Index()
    {
        var itens = ObterCarrinhoDaSessao();
        return View(itens);
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar(Guid remedioId, int quantidade = 1)
    {
        var remedio = await _remedioService.ObterPorIdAsync(remedioId);
        if (remedio != null)
        {
            var itens = ObterCarrinhoDaSessao();
            var itemExistente = itens.FirstOrDefault(i => i.RemedioId == remedioId);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
            }
            else
            {
                itens.Add(new ItemCarrinhoDto
                {
                    RemedioId = remedioId,
                    Nome = remedio.Nome,
                    PrecoUnitario = remedio.Preco, // Mapeia o preço do medicamento
                    Quantidade = quantidade
                });
            }

            SalvarCarrinhoNaSessao(itens);
            TempData["Sucesso"] = "Item adicionado ao carrinho com sucesso!";
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Remover(Guid remedioId)
    {
        var carrinho = ObterCarrinhoDaSessao();
        carrinho.RemoveAll(i => i.RemedioId == remedioId);

        SalvarCarrinhoNaSessao(carrinho);
        TempData["Sucesso"] = "Item removido do carrinho com sucesso!";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> FinalizarPedido(string tipoEntrega, string formaPagamento)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            TempData["Erro"] = "Faça login para finalizar o seu pedido.";
            return RedirectToAction("Login", "Conta", new { returnUrl = Url.Action("Index", "Carrinho") });
        }

        var itens = ObterCarrinhoDaSessao();
        if (!itens.Any())
        {
            TempData["Erro"] = "Seu carrinho está vazio.";
            return RedirectToAction(nameof(Index));
        }

        var clienteIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clienteIdClaim, out var clienteId))
        {
            TempData["Erro"] = "Não foi possível identificar o usuário logado.";
            return RedirectToAction("Login", "Conta");
        }

        var dto = new PedidoCriacaoDto
        {
            ClienteId = clienteId,
            TipoEntrega = tipoEntrega,
            FormaPagamento = formaPagamento,
            Itens = itens
        };

        try
        {
            var pedidoId = await _pedidoService.CriarPedidoAsync(dto);
            HttpContext.Session.Remove(CARRINHO_KEY);
            return RedirectToAction("Confirmacao", new { id = pedidoId });
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return View("Index", itens);
        }
    }

    public IActionResult Confirmacao(Guid id)
    {
        ViewBag.PedidoId = id;
        return View();
    }

    private List<ItemCarrinhoDto> ObterCarrinhoDaSessao()
    {
        var json = HttpContext.Session.GetString(CARRINHO_KEY);
        return string.IsNullOrEmpty(json)
            ? new List<ItemCarrinhoDto>()
            : JsonSerializer.Deserialize<List<ItemCarrinhoDto>>(json) ?? new List<ItemCarrinhoDto>();
    }

    private void SalvarCarrinhoNaSessao(List<ItemCarrinhoDto> itens)
    {
        var json = JsonSerializer.Serialize(itens);
        HttpContext.Session.SetString(CARRINHO_KEY, json);
    }
}