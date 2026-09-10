using ContractFlow.Application.DTOs;
using ContractFlow.Application.Services;
using ContractFlow.Domain.Enums;
using ContractFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContractFlow.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly ContractFlowDbContext _context;

    public DashboardService(ContractFlowDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var today = DateTime.UtcNow.Date;
        var next30Days = today.AddDays(30);

        var contracts = await _context.Contracts
            .AsNoTracking()
            .ToListAsync();

        var totalContracts = contracts.Count;

        var activeContracts = contracts.Count(c =>
            c.Status == ContractStatus.Ativo &&
            c.EndDate.Date >= today
        );

        var expiredContracts = contracts.Count(c =>
            c.Status != ContractStatus.Cancelado &&
            c.Status != ContractStatus.Rejeitado &&
            c.EndDate.Date < today
        );

        var pendingApprovalContracts = contracts.Count(c =>
            c.Status == ContractStatus.AguardandoAprovacao
        );

        var totalValue = contracts
            .Where(c =>
                c.Status != ContractStatus.Cancelado &&
                c.Status != ContractStatus.Rejeitado
            )
            .Sum(c => c.Value);

        var expiringIn30Days = contracts.Count(c =>
            c.Status == ContractStatus.Ativo &&
            c.EndDate.Date >= today &&
            c.EndDate.Date <= next30Days
        );

        return new DashboardDto
        {
            TotalContracts = totalContracts,
            ActiveContracts = activeContracts,
            ExpiredContracts = expiredContracts,
            PendingApprovalContracts = pendingApprovalContracts,
            TotalValue = totalValue,
            ExpiringIn30Days = expiringIn30Days
        };
    }
}