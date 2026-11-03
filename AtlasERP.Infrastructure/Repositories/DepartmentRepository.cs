using AtlasERP.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Application.DTOs.Department;
using AtlasERP.Application.Interfaces.Repositories;
using AtlasERP.Domain.Entities;
using AtlasERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AtlasERP.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AtlasDbContext _context;

    public DepartmentRepository(AtlasDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync(bool includeInactive = false)
    {
        var query = _context.Departments.AsQueryable();

        if (!includeInactive)
            query = query.Where(d => d.IsActive);

        return await query
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                Description = d.Description,
                IsActive = d.IsActive
            })
            .ToListAsync();
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        return await _context.Departments
            .Where(d => d.Id == id)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                Description = d.Description,
                IsActive = d.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Guid> CreateAsync(CreateDepartmentDto dto)
    {
        if (await _context.Departments.AnyAsync(d => d.Code == dto.Code))
            throw new InvalidOperationException($"Department code '{dto.Code}' already exists.");

        var department = new Department
        {
            Code = dto.Code,
            Name = dto.Name,
            Description = dto.Description ?? string.Empty,
            IsActive = true
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return department.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateDepartmentDto dto)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
            return false;

        // if the code is changing, make sure it doesn't collide with a different department
        if (department.Code != dto.Code &&
            await _context.Departments.AnyAsync(d => d.Code == dto.Code))
        {
            throw new InvalidOperationException($"Department code '{dto.Code}' already exists.");
        }

        department.Code = dto.Code;
        department.Name = dto.Name;
        department.Description = dto.Description ?? string.Empty;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
            return false;

        department.IsActive = false;         // soft delete
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}