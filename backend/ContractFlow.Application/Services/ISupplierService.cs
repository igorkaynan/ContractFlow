using ContractFlow.Application.DTOs;

namespace ContractFlow.Application.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<SupplierDto?> GetByIdAsync(Guid id);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateSupplierDto dto);
    Task<bool> DeleteAsync(Guid id);
}