using ContractFlow.Application.DTOs;
using ContractFlow.Application.Services;
using ContractFlow.Domain.Entities;
using ContractFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContractFlow.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly ContractFlowDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AuditService(
        ContractFlowDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task LogAsync(
        string action,
        string entityType,
        Guid entityId,
        string details,
        Guid? userId = null)
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),

            // Se nenhum usuário for informado manualmente,
            // utiliza automaticamente o usuário autenticado pelo JWT.
            UserId = userId ?? _currentUserService.UserId,

            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            CreatedAt = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditLogDto>> GetAllAsync()
    {
        return await _context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AuditLogDto
            {
                Id = a.Id,
                UserId = a.UserId,
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                Details = a.Details,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();
    }
}