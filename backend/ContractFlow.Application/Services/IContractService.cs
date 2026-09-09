using ContractFlow.Application.DTOs;

namespace ContractFlow.Application.Services;

public interface IContractService
{
    Task<IEnumerable<ContractDto>> GetAllAsync();

    Task<ContractDto?> GetByIdAsync(Guid id);

    Task<ContractDto> CreateAsync(CreateContractDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateContractDto dto);

    Task<bool> DeleteAsync(Guid id);
}