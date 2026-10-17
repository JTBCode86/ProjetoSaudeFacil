using SaudeFacil.Application.DTOs;
using SaudeFacil.Application.Interfaces;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Exceptions;
using SaudeFacil.Domain.Interfaces;
using SaudeFacil.Domain.ValueObjects;

namespace SaudeFacil.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<Cliente?> AutenticarAsync(string email, string senha)
    {
        var cliente = await _clienteRepository.ObterPorEmailAsync(email);

        if (cliente == null || cliente.SenhaHash != senha)
            return null;

        return cliente;
    }

    public async Task<Guid> CadastrarAsync(CadastrarClienteDto dto)
    {                                     
        var clienteExistente = await _clienteRepository.ObterPorEmailAsync(dto.Email);
        if (clienteExistente != null)
            throw new DomainException("Já existe um cliente cadastrado com este e-mail.");

        var endereco = new Endereco(
            dto.Cep,
            dto.Logradouro,
            dto.Numero,
            dto.Complemento,
            dto.Bairro,
            dto.Cidade,
            dto.Estado
        );

        // Nota: Em produção, aplique um hash seguro na senha antes de salvar
        var cliente = new Cliente(dto.Nome,dto.Cpf,dto.Email, dto.Senha, dto.Telefone, endereco);
        
        await _clienteRepository.AdicionarAsync(cliente);
        return cliente.Id;
    }

}