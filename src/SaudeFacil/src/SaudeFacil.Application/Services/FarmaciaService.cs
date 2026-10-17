using Microsoft.AspNetCore.Identity;
using SaudeFacil.Application.DTOs;
using SaudeFacil.Application.Interfaces;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Exceptions;
using SaudeFacil.Domain.Interfaces;
using SaudeFacil.Domain.ValueObjects;

namespace SaudeFacil.Application.Services;

public class FarmaciaService : IFarmaciaService
{
    private readonly IFarmaciaRepository _farmaciaRepository;
    private readonly PasswordHasher<object> _passwordHasher = new();

    public FarmaciaService(IFarmaciaRepository farmaciaRepository)
    {
        _farmaciaRepository = farmaciaRepository;
    }

    public async Task CadastrarAsync(CadastrarFarmaciaDto dto)
    {
        var emailLimpo = dto.Email.Trim().ToLower();
        var endereco = new Endereco(dto.Cep, dto.Logradouro, dto.Numero, dto.Complemento, dto.Bairro, dto.Cidade, dto.Estado);

        var hash = _passwordHasher.HashPassword(new object(), dto.Senha);
        var farmacia = new Farmacia(dto.NomeFantasia, dto.Cnpj, emailLimpo, hash, dto.Telefone, endereco);

        await _farmaciaRepository.AdicionarAsync(farmacia);
    }

    public async Task<Guid> CadastrarAsync(FarmaciaCadastroDto dto)
    {
        var farmaciaExistente = await _farmaciaRepository.ObterPorCnpjAsync(dto.CNPJ);
        if (farmaciaExistente != null)
            throw new DomainException("Já existe uma farmácia cadastrada com este CNPJ.");

        var emailLimpo = dto.Email.Trim().ToLower();
        var endereco = new Endereco(
            dto.CEP,
            dto.Logradouro,
            dto.Numero,
            dto.Complemento,
            dto.Bairro,
            dto.Cidade,
            dto.Estado
        );

        var hash = _passwordHasher.HashPassword(new object(), dto.Senha);
        var farmacia = new Farmacia(dto.NomeFantasia, dto.CNPJ, emailLimpo, hash, dto.Telefone, endereco);

        await _farmaciaRepository.AdicionarAsync(farmacia);
        return farmacia.Id;
    }

    public async Task<Farmacia?> AutenticarAsync(string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            return null;

        var emailLimpo = email.Trim().ToLower();

        // Tenta buscar por e-mail no repositório
        var farmacia = await _farmaciaRepository.ObterPorEmailAsync(emailLimpo);

        // Fallback: se o ObterPorEmailAsync falhar no repositório, busca em memória
        if (farmacia == null)
        {
            var todas = await _farmaciaRepository.ObterTodasAsync();
            farmacia = todas.FirstOrDefault(f => f.Email != null && f.Email.Trim().ToLower() == emailLimpo);
        }

        if (farmacia == null)
            return null;

        // 1. Tenta validar via Hash
        try
        {
            if (!string.IsNullOrEmpty(farmacia.SenhaHash))
            {
                var result = _passwordHasher.VerifyHashedPassword(new object(), farmacia.SenhaHash, senha);
                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return farmacia;
                }
            }
        }
        catch
        {
            // Se o valor gravado não for um hash válido
        }

        // 2. Fallback: Texto puro
        if (farmacia.SenhaHash == senha)
        {
            return farmacia;
        }

        return null;
    }

    public async Task<IEnumerable<FarmaciaDto>> ObterTodasAsync()
    {
        var farmacias = await _farmaciaRepository.ObterTodasAsync();

        return farmacias.Select(f => new FarmaciaDto
        {
            Id = f.Id,
            NomeFantasia = f.NomeFantasia,
            Logradouro = f.Endereco?.Logradouro ?? string.Empty,
            Bairro = f.Endereco?.Bairro ?? string.Empty,
            Cidade = f.Endereco?.Cidade ?? string.Empty,
            Latitude = f.Endereco?.Latitude ?? 0,
            Longitude = f.Endereco?.Longitude ?? 0,
            DistanciaKm = f.DistanciaKm
        });
    }
}