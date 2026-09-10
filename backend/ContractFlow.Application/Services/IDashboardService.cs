using ContractFlow.Application.DTOs;

namespace ContractFlow.Application.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}