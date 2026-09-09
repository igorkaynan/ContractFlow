using ContractFlow.Domain.Enums;

namespace ContractFlow.Domain.Entities;

public class Contract
{
    public Guid Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid? SupplierId { get; set; }

    public Supplier? Supplier { get; set; }

    public string SupplierName { get; set; } = string.Empty;

    public decimal Value { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public ContractStatus Status { get; set; }

    public bool AutomaticRenewal { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}