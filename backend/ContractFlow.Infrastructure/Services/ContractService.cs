using ContractFlow.Application.DTOs;
using ContractFlow.Application.Services;
using ContractFlow.Domain.Entities;
using ContractFlow.Domain.Enums;
using ContractFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContractFlow.Infrastructure.Services;

public class ContractService : IContractService
{
    private readonly ContractFlowDbContext _context;

    public ContractService(ContractFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContractDto>> GetAllAsync()
    {
        var contracts = await _context.Contracts
            .Include(c => c.Supplier)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return contracts.Select(c => new ContractDto
        {
            Id = c.Id,
            Number = c.Number,
            Title = c.Title,
            Description = c.Description,
            SupplierId = c.SupplierId,
            SupplierName = c.Supplier?.Name ?? c.SupplierName,
            Value = c.Value,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Status = GetCurrentStatus(c).ToString(),
            AutomaticRenewal = c.AutomaticRenewal,
            CreatedAt = c.CreatedAt
        });
    }

    public async Task<ContractDto?> GetByIdAsync(Guid id)
    {
        var contract = await _context.Contracts
            .Include(c => c.Supplier)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contract is null)
        {
            return null;
        }

        return new ContractDto
        {
            Id = contract.Id,
            Number = contract.Number,
            Title = contract.Title,
            Description = contract.Description,
            SupplierId = contract.SupplierId,
            SupplierName = contract.Supplier?.Name ?? contract.SupplierName,
            Value = contract.Value,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Status = GetCurrentStatus(contract).ToString(),
            AutomaticRenewal = contract.AutomaticRenewal,
            CreatedAt = contract.CreatedAt
        };
    }

    public async Task<ContractDto> CreateAsync(CreateContractDto dto)
    {
        if (dto.EndDate < dto.StartDate)
        {
            throw new ArgumentException(
                "A data final não pode ser anterior à data inicial."
            );
        }

        if (!Enum.TryParse<ContractStatus>(dto.Status, true, out var status))
        {
            throw new ArgumentException("Status inválido.");
        }

        var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);

        if (supplier is null)
        {
            throw new ArgumentException("Fornecedor não encontrado.");
        }

        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            Number = dto.Number,
            Title = dto.Title,
            Description = dto.Description,

            SupplierId = supplier.Id,
            SupplierName = supplier.Name,

            Value = dto.Value,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = status,
            AutomaticRenewal = dto.AutomaticRenewal,
            CreatedAt = DateTime.UtcNow
        };

        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();

        return new ContractDto
        {
            Id = contract.Id,
            Number = contract.Number,
            Title = contract.Title,
            Description = contract.Description,
            SupplierId = contract.SupplierId,
            SupplierName = supplier.Name,
            Value = contract.Value,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Status = GetCurrentStatus(contract).ToString(),
            AutomaticRenewal = contract.AutomaticRenewal,
            CreatedAt = contract.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateContractDto dto)
    {
        if (dto.EndDate < dto.StartDate)
        {
            throw new ArgumentException(
                "A data final não pode ser anterior à data inicial."
            );
        }

        if (!Enum.TryParse<ContractStatus>(dto.Status, true, out var status))
        {
            throw new ArgumentException("Status inválido.");
        }

        var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);

        if (supplier is null)
        {
            throw new ArgumentException("Fornecedor não encontrado.");
        }

        var contract = await _context.Contracts.FindAsync(id);

        if (contract is null)
        {
            return false;
        }

        contract.Number = dto.Number;
        contract.Title = dto.Title;
        contract.Description = dto.Description;

        contract.SupplierId = supplier.Id;
        contract.SupplierName = supplier.Name;

        contract.Value = dto.Value;
        contract.StartDate = dto.StartDate;
        contract.EndDate = dto.EndDate;
        contract.Status = status;
        contract.AutomaticRenewal = dto.AutomaticRenewal;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var contract = await _context.Contracts.FindAsync(id);

        if (contract is null)
        {
            return false;
        }

        _context.Contracts.Remove(contract);
        await _context.SaveChangesAsync();

        return true;
    }

    private static ContractStatus GetCurrentStatus(Contract contract)
    {
        if (contract.Status == ContractStatus.Cancelado)
        {
            return ContractStatus.Cancelado;
        }

        if (contract.EndDate.Date < DateTime.UtcNow.Date)
        {
            return ContractStatus.Vencido;
        }

        return contract.Status;
    }
}