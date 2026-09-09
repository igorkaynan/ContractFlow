using System.ComponentModel.DataAnnotations;

namespace ContractFlow.Application.DTOs;

public class UpdateSupplierDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
}