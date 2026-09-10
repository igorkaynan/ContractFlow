namespace ContractFlow.Application.Services;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}