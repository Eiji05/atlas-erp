using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Application.DTOs.Department;
// IDepartmentRepository.cs
public interface IDepartmentRepository
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync(bool includeInactive = false);   // ← updated
    Task<DepartmentDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateDepartmentDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateDepartmentDto dto);
    Task<bool> DeleteAsync(Guid id);
}