using SaudeFacil.Domain.Entities;
using Xunit;

namespace SaudeFacil.UnitTests.Domain;

public class RemedioTests
{
    [Fact]
    public void CriarRemedio_ComPrecoInvalido_DeveLancarExcecao()
    {
        // Arrange
        var farmaciaId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Remedio(farmaciaId, 1, "Dipirona", "500mg", 0, 10, null, false)
        );
    }

    [Fact]
    public void DeduzirEstoque_ComQuantidadeMaiorQueEstoque_DeveLancarExcecao()
    {
        // Arrange
        var remedio = new Remedio(Guid.NewGuid(), 1, "Paracetamol", "750mg", 15.50m, 5, null, false);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => remedio.DeduzirEstoque(10));
    }

    [Fact]
    public void DeduzirEstoque_ComQuantidadeValida_DeveAtualizarEstoque()
    {
        // Arrange
        var remedio = new Remedio(Guid.NewGuid(), 1, "Paracetamol", "750mg", 15.50m, 10, null, false);

        // Act
        remedio.DeduzirEstoque(3);

        // Assert
        Assert.Equal(7, remedio.Estoque);
    }
}