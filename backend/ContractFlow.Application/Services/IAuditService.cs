using ContractFlow.Application.DTOs;

namespace ContractFlow.Application.Services;

public interface IAuditService
{
    Task LogAsync(
        string action,
        string entityType,
        Guid entityId,
        string details,
        Guid? userId = null
    );

    Task<IEnumerable<AuditLogDto>> GetAllAsync();
}