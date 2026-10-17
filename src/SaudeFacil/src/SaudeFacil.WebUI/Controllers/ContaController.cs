using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SaudeFacil.Application.DTOs;
using SaudeFacil.Application.Interfaces;
using SaudeFacil.Domain.Exceptions;

namespace SaudeFacil.WebUI.Controllers;

public class ContaController : Controller
{
    private readonly IClienteService _clienteService;
    private readonly IFarmaciaService _farmaciaService;

    public ContaController(IClienteService clienteService, IFarmaciaService farmaciaService)
    {
        _clienteService = clienteService;
        _farmaciaService = farmaciaService;
    }

    // GET: /Conta/Login
    public IActionResult Login() => View();

    // POST: /Conta/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var emailLimpo = dto.Email.Trim().ToLower();

            // Tenta autenticar Farmácia
            var farmacia = await _farmaciaService.AutenticarAsync(emailLimpo, dto.Senha);
            if (farmacia != null)
            {
                await CriarSessaoAutenticadaAsync(farmacia.Id.ToString(), farmacia.NomeFantasia, emailLimpo, "Farmacia");
                TempData["Sucesso"] = $"Bem-vinda, {farmacia.NomeFantasia}!";
                return RedirectToAction("Index", "Home");
            }

            // Tenta autenticar Cliente
            var cliente = await _clienteService.AutenticarAsync(emailLimpo, dto.Senha);
            if (cliente != null)
            {
                await CriarSessaoAutenticadaAsync(cliente.Id.ToString(), cliente.Nome, emailLimpo, "Cliente");
                TempData["Sucesso"] = $"Bem-vindo, {cliente.Nome}!";
                return RedirectToAction("Index", "Home");
            }

            // Diagnóstico exato do motivo da falha
            var todasFarmacias = await _farmaciaService.ObterTodasAsync();
            var farmaciaEncontrada = todasFarmacias.FirstOrDefault(f => f.Email != null && f.Email.Trim().ToLower() == emailLimpo);

            if (farmaciaEncontrada == null)
            {
                ModelState.AddModelError(string.Empty, $"Diagnóstico: O e-mail '{dto.Email}' NÃO foi encontrado na tabela Farmacias.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Diagnóstico: E-mail encontrado! Mas a senha enviada '{dto.Senha}' não coincide com a cadastrada '{farmaciaEncontrada.SenhaHash}'.");
            }

            return View(dto);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro: {ex.Message}");
            return View(dto);
        }
    }

    // GET: /Conta/CadastrarCliente
    public IActionResult CadastrarCliente() => View();

    // POST: /Conta/CadastrarCliente
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CadastrarCliente(CadastrarClienteDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _clienteService.CadastrarAsync(dto);

            TempData["Sucesso"] = "Cadastro de cliente realizado com sucesso! Faça login para continuar.";
            return RedirectToAction("Login");
        }
        catch (DomainException ex)
        {
            // Captura o erro de e-mail ou CPF duplicado e envia para a View
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao realizar o cadastro.");
            return View(dto);
        }
    }

    // GET: /Conta/CadastrarFarmacia
    public IActionResult CadastrarFarmacia() => View();

    // POST: /Conta/CadastrarFarmacia
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CadastrarFarmacia(FarmaciaCadastroDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _farmaciaService.CadastrarAsync(dto);

            TempData["Sucesso"] = "Farmácia cadastrada com sucesso! Faça login para gerenciar seu estoque.";
            return RedirectToAction("Login");
        }
        catch (DomainException ex)
        {
            // Captura o erro de e-mail ou CNPJ duplicado e envia para a View
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao realizar o cadastro da farmácia.");
            return View(dto);
        }
    }

    // POST: /Conta/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Sucesso"] = "Sessão encerrada com sucesso.";
        return RedirectToAction("Index", "Home");
    }

    // GET: /Conta/AcessoNegado
    public IActionResult AcessoNegado() => View();

    private async Task CriarSessaoAutenticadaAsync(string id, string nome, string email, string perfil)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Name, nome),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, perfil)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}