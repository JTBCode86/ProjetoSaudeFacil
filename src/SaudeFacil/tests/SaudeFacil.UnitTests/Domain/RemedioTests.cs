using Moq;
using SaudeFacil.Application.Services;
using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.Interfaces;
using Xunit;

namespace SaudeFacil.UnitTests.Application;

public class RemedioServiceTests
{
    [Fact]
    public async Task BuscarRemediosAsync_DeveRetornarListaDeDtos()
    {
        // Arrange
        var mockRepo = new Mock<IRemedioRepository>();
        var farmaciaId = Guid.NewGuid();

        var listaRemedios = new List<Remedio>
        {
            new Remedio(farmaciaId, 1, "Omeprazol", "20mg", 12.00m, 20, null, false)
        };

        mockRepo.Setup(r => r.BuscarPorNomeOuCategoriaAsync("Omeprazol", null))
                .ReturnsAsync(listaRemedios);

        var service = new RemedioService(mockRepo.Object);

        // Act
        var resultado = await service.BuscarRemediosAsync("Omeprazol", null);

        // Assert
        Assert.NotNull(resultado);
        Assert.Single(resultado);
        Assert.Equal("Omeprazol", resultado.First().Nome);
    }
}