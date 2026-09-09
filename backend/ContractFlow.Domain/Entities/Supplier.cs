namespace ContractFlow.Domain.Entities;

public class Supplier
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Cnpj { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}