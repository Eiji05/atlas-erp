using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Application.DTOs.Employee;

namespace AtlasERP.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync(bool includeInactive = false);
    Task<EmployeeDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateEmployeeDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateEmployeeDto dto);
    Task<bool> DeleteAsync(Guid id);
}