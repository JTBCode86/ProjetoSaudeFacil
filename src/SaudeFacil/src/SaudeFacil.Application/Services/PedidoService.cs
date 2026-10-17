using SaudeFacil.Application.DTOs;
using SaudeFacil.Application.Interfaces;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Exceptions;
using SaudeFacil.Domain.Interfaces;

namespace SaudeFacil.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IRemedioRepository _remedioRepository;
    private readonly IClienteRepository _clienteRepository;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IRemedioRepository remedioRepository,
        IClienteRepository clienteRepository)
    {
        _pedidoRepository = pedidoRepository;
        _remedioRepository = remedioRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<Guid> CriarPedidoAsync(PedidoCriacaoDto dto)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(dto.ClienteId);
        if (cliente == null)
            throw new DomainException("Cliente não encontrado.");

        if (dto.Itens == null || !dto.Itens.Any())
            throw new DomainException("O pedido precisa conter pelo menos um item.");

        var pedido = new Pedido(dto.ClienteId, dto.TipoEntrega, dto.FormaPagamento);

        foreach (var itemDto in dto.Itens)
        {
            var remedio = await _remedioRepository.ObterPorIdAsync(itemDto.RemedioId);
            if (remedio == null)
                throw new DomainException($"Remédio de ID '{itemDto.RemedioId}' não encontrado.");

            // Adiciona o item e reduz o estoque na entidade Remedio
            pedido.AdicionarItem(remedio, itemDto.Quantidade);

            // Atualiza a entidade Remedio com o novo estoque
            _remedioRepository.Atualizar(remedio);
        }

        await _pedidoRepository.AdicionarAsync(pedido);
        return pedido.Id;
    }
}