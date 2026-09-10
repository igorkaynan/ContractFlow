namespace ContractFlow.Application.DTOs;

public class DashboardDto
{
    public int TotalContracts { get; set; }
    public int ActiveContracts { get; set; }
    public int ExpiredContracts { get; set; }
    public int PendingApprovalContracts { get; set; }
    public decimal TotalValue { get; set; }
    public int ExpiringIn30Days { get; set; }
}