using AtlasERP.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AtlasERP.Application.DTOs.Employee;
using AtlasERP.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace AtlasERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _repository;

    public EmployeeController(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll([FromQuery] bool includeInactive = false)
    {
        var employees = await _repository.GetAllAsync(includeInactive);
        HideSalaryIfNotAdmin(employees);
        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            return NotFound();

        HideSalaryIfNotAdmin(new[] { employee });
        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {
        try
        {
            var id = await _repository.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update(Guid id, UpdateEmployeeDto dto)
    {
        try
        {
            var updated = await _repository.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private void HideSalaryIfNotAdmin(IEnumerable<EmployeeDto> employees)
    {
        if (User.IsInRole("Administrator"))
            return;

        foreach (var employee in employees)
            employee.Salary = null;
    }
}