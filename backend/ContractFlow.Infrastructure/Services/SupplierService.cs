using ContractFlow.Application.DTOs;
using ContractFlow.Application.Services;
using ContractFlow.Domain.Entities;
using ContractFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContractFlow.Infrastructure.Services;

public class SupplierService : ISupplierService
{
    private readonly ContractFlowDbContext _context;

    public SupplierService(ContractFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        return await _context.Suppliers
            .OrderBy(s => s.Name)
            .Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                Cnpj = s.Cnpj,
                Email = s.Email,
                Phone = s.Phone,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<SupplierDto?> GetByIdAsync(Guid id)
    {
        return await _context.Suppliers
            .Where(s => s.Id == id)
            .Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                Cnpj = s.Cnpj,
                Email = s.Email,
                Phone = s.Phone,
                CreatedAt = s.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
    {
        var exists = await _context.Suppliers
            .AnyAsync(s => s.Cnpj == dto.Cnpj);

        if (exists)
        {
            throw new ArgumentException("Já existe um fornecedor com este CNPJ.");
        }

        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Cnpj = dto.Cnpj,
            Email = dto.Email,
            Phone = dto.Phone,
            CreatedAt = DateTime.UtcNow
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Cnpj = supplier.Cnpj,
            Email = supplier.Email,
            Phone = supplier.Phone,
            CreatedAt = supplier.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateSupplierDto dto)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier is null)
        {
            return false;
        }

        var exists = await _context.Suppliers
            .AnyAsync(s => s.Cnpj == dto.Cnpj && s.Id != id);

        if (exists)
        {
            throw new ArgumentException("Já existe outro fornecedor com este CNPJ.");
        }

        supplier.Name = dto.Name;
        supplier.Cnpj = dto.Cnpj;
        supplier.Email = dto.Email;
        supplier.Phone = dto.Phone;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier is null)
        {
            return false;
        }

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return true;
    }
}