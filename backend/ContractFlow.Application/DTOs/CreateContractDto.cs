using System.ComponentModel.DataAnnotations;

namespace ContractFlow.Application.DTOs;

public class CreateContractDto
{
    [Required]
    public string Number { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid SupplierId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Value { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    public bool AutomaticRenewal { get; set; }
}