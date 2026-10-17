using SaudeFacil.Domain.Entities;
using Xunit;

namespace SaudeFacil.UnitTests.Domain;

public class PedidoTests
{
    [Fact]
    public void AdicionarItem_DeveCalcularValorTotalEDeduzirEstoque()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var pedido = new Pedido(clienteId, "Entrega", "PIX");
        var remedio = new Remedio(Guid.NewGuid(), 1, "Ibuprofeno", "600mg", 20.00m, 15, null, false);

        // Act
        pedido.AdicionarItem(remedio, 2);

        // Assert
        Assert.Single(pedido.Itens);
        Assert.Equal(40.00m, pedido.ValorTotal);
        Assert.Equal(13, remedio.Estoque);
    }
}