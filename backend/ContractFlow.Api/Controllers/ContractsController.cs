using ContractFlow.Application.DTOs;
using ContractFlow.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContractDto>>> GetAll()
    {
        var contracts = await _contractService.GetAllAsync();

        return Ok(contracts);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContractDto>> GetById(Guid id)
    {
        var contract = await _contractService.GetByIdAsync(id);

        if (contract is null)
        {
            return NotFound();
        }

        return Ok(contract);
    }

    [HttpPost]
    public async Task<ActionResult<ContractDto>> Create(CreateContractDto dto)
    {
        try
        {
            var contract = await _contractService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = contract.Id },
                contract
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateContractDto dto)
    {
        try
        {
            var updated = await _contractService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _contractService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}