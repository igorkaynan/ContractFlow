using ContractFlow.Domain.Entities;
using ContractFlow.Domain.Enums;

namespace ContractFlow.Tests;

public class ContractTests
{
    [Fact]
    public void Contract_Should_Create_With_Active_Status()
    {
        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "CTR-TEST-001",
            Title = "Contrato de Teste",
            Value = 1000,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1),
            Status = ContractStatus.Ativo
        };

        Assert.Equal(ContractStatus.Ativo, contract.Status);
        Assert.Equal(1000, contract.Value);
        Assert.True(contract.EndDate > contract.StartDate);
    }

    [Fact]
    public void Contract_Should_Allow_Awaiting_Approval_Status()
    {
        var contract = new Contract
        {
            Status = ContractStatus.AguardandoAprovacao
        };

        Assert.Equal(
            ContractStatus.AguardandoAprovacao,
            contract.Status
        );
    }

    [Fact]
    public void Contract_Should_Allow_Rejected_Status()
    {
        var contract = new Contract
        {
            Status = ContractStatus.Rejeitado
        };

        Assert.Equal(
            ContractStatus.Rejeitado,
            contract.Status
        );
    }
}