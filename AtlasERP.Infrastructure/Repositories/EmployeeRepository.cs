using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Application.DTOs.Employee;
using AtlasERP.Application.Interfaces.Repositories;
using AtlasERP.Domain.Entities;
using AtlasERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AtlasERP.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AtlasDbContext _context;

    public EmployeeRepository(AtlasDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(bool includeInactive = false)
    {
        var query = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .AsQueryable();

        if (!includeInactive)
            query = query.Where(e => e.IsActive);

        return await query
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                EmployeeNumber = e.EmployeeNumber,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                BirthDate = e.BirthDate,
                HireDate = e.HireDate,
                Salary = e.Salary,
                IsActive = e.IsActive,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department != null ? e.Department.Name : string.Empty,
                PositionId = e.PositionId,
                PositionName = e.Position != null ? e.Position.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Where(e => e.Id == id)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                EmployeeNumber = e.EmployeeNumber,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                BirthDate = e.BirthDate,
                HireDate = e.HireDate,
                Salary = e.Salary,
                IsActive = e.IsActive,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department != null ? e.Department.Name : string.Empty,
                PositionId = e.PositionId,
                PositionName = e.Position != null ? e.Position.Name : string.Empty
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Guid> CreateAsync(CreateEmployeeDto dto)
    {
        if (!await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
            throw new InvalidOperationException("The specified department does not exist.");

        if (!await _context.Positions.AnyAsync(p => p.Id == dto.PositionId))
            throw new InvalidOperationException("The specified position does not exist.");

        if (await _context.Employees.AnyAsync(e => e.EmployeeNumber == dto.EmployeeNumber))
            throw new InvalidOperationException($"Employee number '{dto.EmployeeNumber}' already exists.");

        if (await _context.Employees.AnyAsync(e => e.Email == dto.Email))
            throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");

        var employee = new Employee
        {
            EmployeeNumber = dto.EmployeeNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            BirthDate = dto.BirthDate,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            DepartmentId = dto.DepartmentId,
            PositionId = dto.PositionId,
            IsActive = true
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return employee.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            return false;

        if (!await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
            throw new InvalidOperationException("The specified department does not exist.");

        if (!await _context.Positions.AnyAsync(p => p.Id == dto.PositionId))
            throw new InvalidOperationException("The specified position does not exist.");

        if (employee.Email != dto.Email &&
            await _context.Employees.AnyAsync(e => e.Email == dto.Email))
        {
            throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");
        }

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.Salary = dto.Salary;
        employee.DepartmentId = dto.DepartmentId;
        employee.PositionId = dto.PositionId;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            return false;

        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}