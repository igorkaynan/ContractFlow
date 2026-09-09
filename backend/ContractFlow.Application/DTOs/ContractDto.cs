namespace ContractFlow.Application.DTOs;

public class ContractDto
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid? SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;

    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool AutomaticRenewal { get; set; }
    public DateTime CreatedAt { get; set; }
}