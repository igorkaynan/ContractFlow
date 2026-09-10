using ContractFlow.Application.DTOs;
using ContractFlow.Application.Services;
using ContractFlow.Domain.Entities;
using ContractFlow.Domain.Enums;
using ContractFlow.Infrastructure.Persistence;
using ContractFlow.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ContractFlow.Tests;

public class ContractServiceTests
{
    private static ContractFlowDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ContractFlowDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ContractFlowDbContext(options);
    }

    private static ContractService CreateService(
        ContractFlowDbContext context)
    {
        var currentUserService = new FakeCurrentUserService();

        var auditService = new AuditService(
            context,
            currentUserService
        );

        return new ContractService(
            context,
            auditService
        );
    }

    [Fact]
    public async Task Approve_Should_Activate_Pending_Contract()
    {
        await using var context = CreateContext();

        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "TEST-001",
            Title = "Contrato aguardando aprovação",
            Status = ContractStatus.AguardandoAprovacao,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1)
        };

        context.Contracts.Add(contract);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.ApproveAsync(contract.Id);

        Assert.True(result);
        Assert.Equal(
            ContractStatus.Ativo,
            contract.Status
        );
    }

    [Fact]
    public async Task Approve_Should_Reject_Already_Active_Contract()
    {
        await using var context = CreateContext();

        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "TEST-002",
            Title = "Contrato ativo",
            Status = ContractStatus.Ativo,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1)
        };

        context.Contracts.Add(contract);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.ApproveAsync(contract.Id)
        );
    }

    [Fact]
    public async Task Reject_Should_Reject_Pending_Contract()
    {
        await using var context = CreateContext();

        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "TEST-003",
            Title = "Contrato para rejeição",
            Status = ContractStatus.AguardandoAprovacao,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1)
        };

        context.Contracts.Add(contract);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.RejectAsync(contract.Id);

        Assert.True(result);
        Assert.Equal(
            ContractStatus.Rejeitado,
            contract.Status
        );
    }

    [Fact]
    public async Task Create_Should_Reject_When_EndDate_Is_Before_StartDate()
    {
        await using var context = CreateContext();

        var service = CreateService(context);

        var dto = new CreateContractDto
        {
            Number = "TEST-004",
            Title = "Contrato com data inválida",
            SupplierId = Guid.NewGuid(),
            Value = 1000,
            StartDate = new DateTime(2026, 10, 1),
            EndDate = new DateTime(2026, 9, 1),
            Status = "Ativo"
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync(dto)
        );

        Assert.Equal(
            "A data final não pode ser anterior à data inicial.",
            exception.Message
        );
    }

    [Fact]
    public async Task Create_Should_Reject_When_Supplier_Does_Not_Exist()
    {
        await using var context = CreateContext();

        var service = CreateService(context);

        var dto = new CreateContractDto
        {
            Number = "TEST-005",
            Title = "Contrato sem fornecedor",
            SupplierId = Guid.NewGuid(),
            Value = 1000,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1),
            Status = "Ativo"
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync(dto)
        );

        Assert.Equal(
            "Fornecedor não encontrado.",
            exception.Message
        );
    }

    private class FakeCurrentUserService : ICurrentUserService
    {
        public Guid? UserId { get; } = Guid.NewGuid();
    }
}