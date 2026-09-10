using ContractFlow.Domain.Entities;
using ContractFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ContractFlow.Infrastructure.Persistence;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(ContractFlowDbContext context)
    {
        var today = DateTime.UtcNow.Date;

        // Se os dados de demonstração já existem, mantém três contratos ativos
        // com vencimento dentro dos próximos 30 dias para demonstrar o alerta do Dashboard.
        if (await context.Contracts.AnyAsync(c => c.Number.StartsWith("DEMO-")))
        {
            var demoContracts = await context.Contracts
                .Where(c => c.Number == "DEMO-001" || c.Number == "DEMO-002" || c.Number == "DEMO-003")
                .OrderBy(c => c.Number)
                .ToListAsync();

            var days = new[] { 7, 18, 29 };
            for (var i = 0; i < demoContracts.Count && i < days.Length; i++)
            {
                demoContracts[i].Status = ContractStatus.Ativo;
                demoContracts[i].EndDate = today.AddDays(days[i]);
            }

            await context.SaveChangesAsync();
            return;
        }

        var companies = new[]
        {
            "Nexa Tecnologia Ltda", "Vértice Soluções Corporativas", "Lumina Energia Brasil", "Atlas Serviços Integrados",
            "Orbe Sistemas Empresariais", "Prisma Telecomunicações", "Horizonte Facilities", "Metrópole Logística",
            "Integra Cloud Solutions", "Ponto Norte Consultoria", "Elo Segurança Digital", "Via Magna Transportes",
            "CoreData Analytics", "Aliança Infraestrutura", "BluePeak Software", "Nova Era Suprimentos",
            "Sigma Gestão Empresarial", "UrbanTech Serviços", "Conecta Business Solutions", "Prime Office Facilities"
        };

        var titles = new[]
        {
            "Licenciamento de software corporativo", "Suporte e sustentação de sistemas", "Fornecimento de energia e manutenção",
            "Serviços administrativos terceirizados", "Plataforma de gestão empresarial", "Links de internet e telefonia",
            "Manutenção predial e facilities", "Operação logística e distribuição", "Infraestrutura em nuvem",
            "Consultoria estratégica empresarial", "Segurança da informação e monitoramento", "Transporte corporativo",
            "Analytics e inteligência de dados", "Manutenção de infraestrutura", "Desenvolvimento e sustentação de software",
            "Fornecimento de materiais corporativos", "Consultoria em processos e gestão", "Serviços de tecnologia urbana",
            "Integração de sistemas corporativos", "Gestão de escritórios e facilities"
        };

        decimal[] values =
        {
            18437.62m, 27184.39m, 42317.85m, 15892.47m, 36741.28m,
            21954.73m, 48962.14m, 33418.91m, 57743.26m, 12684.55m,
            29371.68m, 44628.37m, 61984.52m, 17346.89m, 52817.43m,
            24139.76m, 38954.21m, 68472.93m, 31427.58m, 55263.17m
        };

        var suppliers = new List<Supplier>();
        for (var i = 0; i < companies.Length; i++)
        {
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                Name = companies[i],
                Cnpj = $"{20 + i:00}.{100 + i:000}.{200 + i:000}/0001-{10 + i:00}",
                Email = $"contato{i + 1:00}@empresa-demo.com.br",
                Phone = $"(11) 9{8000 + i:0000}-{1000 + i:0000}",
                CreatedAt = DateTime.UtcNow.AddDays(-(40 - i))
            };
            suppliers.Add(supplier);
        }

        context.Suppliers.AddRange(suppliers);

        var contracts = new List<Contract>();
        for (var i = 0; i < suppliers.Count; i++)
        {
            var status = i switch
            {
                3 or 11 or 17 => ContractStatus.AguardandoAprovacao,
                6 or 14 => ContractStatus.Rascunho,
                _ => ContractStatus.Ativo
            };

            var start = today.AddMonths(-(i % 10 + 1));
            var end = i switch
            {
                0 => today.AddDays(7),
                1 => today.AddDays(18),
                2 => today.AddDays(29),
                4 or 12 => today.AddDays(-(15 + i)),
                _ => today.AddMonths(6 + (i % 12))
            };

            contracts.Add(new Contract
            {
                Id = Guid.NewGuid(),
                Number = $"DEMO-{i + 1:000}",
                Title = titles[i],
                Description = $"Contrato de demonstração com {companies[i]} para composição do portfólio ContractFlow.",
                SupplierId = suppliers[i].Id,
                SupplierName = suppliers[i].Name,
                Value = values[i],
                StartDate = start,
                EndDate = end,
                Status = status,
                AutomaticRenewal = i % 3 == 0,
                CreatedAt = DateTime.UtcNow.AddDays(-i)
            });
        }

        context.Contracts.AddRange(contracts);
        await context.SaveChangesAsync();
    }
}
