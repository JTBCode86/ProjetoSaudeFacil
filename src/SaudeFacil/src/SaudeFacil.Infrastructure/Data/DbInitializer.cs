using SaudeFacil.Domain.Entities;
using SaudeFacil.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace SaudeFacil.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(SaudeFacilDbContext context)
    {
        // 1. Garante que as Farmácias e Categorias existem
        if (!await context.Farmacias.AnyAsync())
        {
            var end1 = new Endereco("01310-100", "Av. Paulista", "1000", null, "Bela Vista", "São Paulo", "SP", -23.5616, -46.6560);
            var end2 = new Endereco("04538-132", "Rua Cluch", "500", null, "Itaim Bibi", "São Paulo", "SP", -23.5841, -46.6815);
            var end3 = new Endereco("05422-000", "Rua Pinheiros", "250", null, "Pinheiros", "São Paulo", "SP", -23.5667, -46.7022);
            var end4 = new Endereco("03162-000", "Rua da Mooca", "330", null, "Mooca", "São Paulo", "SP", -23.5558, -46.6083);
            var end5 = new Endereco("04010-000", "Rua Domingos de Morais", "500", null, "Vila Mariana", "São Paulo", "SP", -23.5750, -46.6397);
            var end6 = new Endereco("02011-000", "Rua Voluntários da Pátria", "1500", null, "Santana", "São Paulo", "SP", -23.5015, -46.6267);

            var farmaciaA = new Farmacia("Farmácia Saúde & Cia", "12345678000199", "contato@saudeecia.com", "hash123", "(11) 98888-1111", end1);
            var farmaciaB = new Farmacia("Drogaria Popular do Bairro", "98765432000188", "atendimento@drogariapopular.com", "hash123", "(11) 97777-2222", end2);
            var farmaciaC = new Farmacia("Pharma Viva", "45678912000177", "vendas@pharmaviva.com", "hash123", "(11) 96666-3333", end3);
            var farmaciaD = new Farmacia("Drogaria Pague Menos Bairro", "33445566000144", "contato@paguemenosbairro.com", "hash123", "(11) 95555-4444", end4);
            var farmaciaE = new Farmacia("Farmácia Bem Estar", "55667788000155", "atendimento@farmaciabemestar.com", "hash123", "(11) 94444-5555", end5);
            var farmaciaF = new Farmacia("Drogaria Vida & Saude", "77889900000166", "vendas@vidasaudefarmacia.com", "hash123", "(11) 93333-6666", end6);

            await context.Farmacias.AddRangeAsync(farmaciaA, farmaciaB, farmaciaC, farmaciaD, farmaciaE, farmaciaF);

            var catAnalgesico = new Categoria("Analgésicos e Térmicos", "Alívio de dores e febre");
            var catAntiinflamatorio = new Categoria("Anti-inflamatórios", "Tratamento de inflamações");
            var catVitaminas = new Categoria("Vitaminas e Suplementos", "Cuidados com a saúde diária");

            await context.Categorias.AddRangeAsync(catAnalgesico, catAntiinflamatorio, catVitaminas);
            await context.SaveChangesAsync();
        }

        // 2. Se houver menos de 12 remédios, recria a lista completa
        var totalRemedios = await context.Remedios.CountAsync();
        if (totalRemedios < 12)
        {
            var farmacias = await context.Farmacias.ToListAsync();
            var categorias = await context.Categorias.ToListAsync();

            if (farmacias.Any() && categorias.Any())
            {
                // Limpa registos antigos para evitar duplicados ou contagens incorretas
                var antigos = await context.Remedios.ToListAsync();
                context.Remedios.RemoveRange(antigos);
                await context.SaveChangesAsync();

                var fA = farmacias[0].Id;
                var fB = farmacias.Count > 1 ? farmacias[1].Id : fA;
                var fC = farmacias.Count > 2 ? farmacias[2].Id : fA;
                var fD = farmacias.Count > 3 ? farmacias[3].Id : fA;
                var fE = farmacias.Count > 4 ? farmacias[4].Id : fA;
                var fF = farmacias.Count > 5 ? farmacias[5].Id : fA;

                var cAnalgesico = categorias.First(c => c.Nome.Contains("Analgésicos")).Id;
                var cAntiinflamatorio = categorias.First(c => c.Nome.Contains("Anti-inflamatórios")).Id;
                var cVitaminas = categorias.First(c => c.Nome.Contains("Vitaminas")).Id;

                var remedios = new List<Remedio>
                {
                    new Remedio(fA, cAnalgesico, "Dipirona Monidratada 500mg", "Alívio de dores e febre em geral. Embalagem com 10 comprimidos.", 8.50m, 50, null, false),
                    new Remedio(fB, cAnalgesico, "Dipirona Monidratada 500mg", "Alívio rápido de febre e dores intensas.", 6.90m, 30, null, false),
                    new Remedio(fC, cAnalgesico, "Dorflex 36 Comprimidos", "Analgesico e relaxante muscular para dores no corpo.", 22.90m, 40, null, false),
                    new Remedio(fD, cAntiinflamatorio, "Ibuprofeno 600mg", "Indicado para alívio de inflamações e dores articulares.", 18.20m, 25, null, false),
                    new Remedio(fE, cVitaminas, "Vitamina C 1g Efervescente", "Suplemento vitamínico para fortalecimento da imunidade.", 14.99m, 60, null, false),
                    new Remedio(fF, cAnalgesico, "Paracetamol 750mg", "Alívio de dores leves a moderadas e febre. Caixinha com 20 comprimidos.", 9.80m, 45, null, false),
                    new Remedio(fA, cAntiinflamatorio, "Nimesulida 100mg", "Indicado para o tratamento de dores agudas e inflamações.", 15.50m, 35, null, false),
                    new Remedio(fB, cAntiinflamatorio, "Diclofenaco Sódico 50mg", "Anti-inflamatório indicado para reumatismo e dores articulares.", 11.20m, 20, null, false),
                    new Remedio(fC, cVitaminas, "Multivitamínico A-Z 60 Cápsulas", "Suplemento alimentar com vitaminas e minerais de A a Z.", 39.90m, 50, null, false),
                    new Remedio(fD, cVitaminas, "Vitamina D3 2.000 UI", "Auxilia na absorção de cálcio e no fortalecimento dos ossos.", 28.70m, 40, null, false),
                    new Remedio(fE, cAnalgesico, "Neosaldina 20 Drágeas", "Analgesico indicado para o alívio rápido de dores de cabeça.", 21.50m, 60, null, false),
                    new Remedio(fF, cAntiinflamatorio, "Cataflampro Emulgel 60g", "Alívio local da dor e inflamação em músculos e articulações.", 34.90m, 15, null, false)
                };

                await context.Remedios.AddRangeAsync(remedios);
                await context.SaveChangesAsync();
            }
        }
    }
}