using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaudeFacil.Application.Interfaces;
using SaudeFacil.Application.Utils;
using SaudeFacil.Infrastructure.Data;

namespace SaudeFacil.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IRemedioService _remedioService;
    private readonly IFarmaciaService _farmaciaService;
    private readonly SaudeFacilDbContext _context;

    public HomeController(
        IRemedioService remedioService,
        IFarmaciaService farmaciaService,
        SaudeFacilDbContext context)
    {
        _remedioService = remedioService;
        _farmaciaService = farmaciaService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Garante que o Seeding seja verificado/executado caso a base não tenha os 12 medicamentos
        await DbInitializer.SeedAsync(_context);

        // Procura todos os remédios sem aplicar filtros de termo ou categoria
        var remedios = await _remedioService.BuscarRemediosAsync(null, null);

        return View(remedios);
    }

    public async Task<IActionResult> Buscar(string? termo, int? categoriaId, Guid? farmaciaId)
    {
        if (farmaciaId.HasValue && farmaciaId.Value != Guid.Empty)
        {
            return RedirectToAction(nameof(PorFarmacia), new { id = farmaciaId.Value });
        }

        var remedios = await _remedioService.BuscarRemediosAsync(termo, categoriaId);

        var categorias = await _context.Categorias.ToListAsync();
        ViewBag.Categorias = new SelectList(categorias, "Id", "Nome", categoriaId);
        ViewBag.CategoriaSelecionada = categoriaId;
        ViewBag.TermoBusca = termo;

        return View(remedios);
    }

    [HttpGet]
    public async Task<IActionResult> PorFarmacia(Guid id)
    {
        // Busca a farmácia pelo Guid
        var farmacia = await _context.Farmacias.FirstOrDefaultAsync(f => f.Id == id);

        if (farmacia == null)
        {
            return NotFound();
        }

        // Mapeia os remédios para RemedioConsultaDto
        var remedios = await _context.Remedios
            .Where(r => r.FarmaciaId == id)
            .Include(r => r.Categoria)
            .Include(r => r.Farmacia)
            .Select(r => new SaudeFacil.Application.DTOs.RemedioConsultaDto
            {
                Id = r.Id,
                Nome = r.Nome,
                Descricao = r.Descricao,
                Preco = r.Preco,
                ExigeReceita = r.ExigeReceita,
                MediaAvaliacoes = 5.0, // Valor padrão de avaliação
                CategoriaNome = r.Categoria != null ? r.Categoria.Nome : string.Empty,
                NomeFarmacia = r.Farmacia != null ? r.Farmacia.NomeFantasia : string.Empty,
                BairroFarmacia = string.Empty // Ajustado para evitar o erro de propriedade inexistente
            })
            .ToListAsync();

        // Carrega as categorias para o SelectList da View
        var categorias = await _context.Categorias.ToListAsync();
        ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");

        ViewBag.NomeFarmacia = farmacia.NomeFantasia;

        return View("Buscar", remedios);
    }

    public async Task<IActionResult> MaisProximas(string? lat, string? lon, string? lng)
    {
        double? latitude = null;
        double? longitude = null;

        string? lonStr = lon ?? lng;

        if (!string.IsNullOrEmpty(lat) && double.TryParse(lat, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedLat))
        {
            latitude = parsedLat;
        }

        if (!string.IsNullOrEmpty(lonStr) && double.TryParse(lonStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedLon))
        {
            longitude = parsedLon;
        }

        // Coordenadas padrão (Centro de São Paulo) caso o GPS não venha preenchido
        if (!latitude.HasValue || !longitude.HasValue)
        {
            latitude = -23.550520;
            longitude = -46.633308;
        }

        // 1. Obtém as farmácias já mapeadas em DTO
        var farmaciasDto = (await _farmaciaService.ObterTodasAsync()).ToList(); 

        // 2. Calcula e atribui diretamente na propriedade DistanciaKm de cada DTO
        foreach (var farmacia in farmaciasDto)
        {
            // O valor calculado tem de ser atribuído diretamente à propriedade do DTO
            farmacia.DistanciaKm = GeoUtils.CalcularDistanciaKm(
                latitude.Value,
                longitude.Value,
                farmacia.Latitude,
                farmacia.Longitude
            );
        }

        // 3. Ordena por proximidade
        var farmaciasOrdenadas = farmaciasDto.OrderBy(f => f.DistanciaKm).ToList();
        ViewBag.LocalizacaoObtida = true;

        return View(farmaciasOrdenadas);
    }

    // GET: /Home/Sobre
    public IActionResult Sobre()
    {
        return View();
    }
}