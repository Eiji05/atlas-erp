using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Application.DTOs.Position;
using AtlasERP.Application.Interfaces.Repositories;
using AtlasERP.Domain.Entities;
using AtlasERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AtlasERP.Infrastructure.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly AtlasDbContext _context;

    public PositionRepository(AtlasDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PositionDto>> GetAllAsync(bool includeInactive = false)
    {
        var query = _context.Positions
            .Include(p => p.Department)
            .AsQueryable();

        if (!includeInactive)
            query = query.Where(p => p.IsActive);

        return await query
            .Select(p => new PositionDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive,
                DepartmentId = p.DepartmentId,
                DepartmentName = p.Department != null ? p.Department.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<PositionDto?> GetByIdAsync(Guid id)
    {
        return await _context.Positions
            .Include(p => p.Department)
            .Where(p => p.Id == id)
            .Select(p => new PositionDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive,
                DepartmentId = p.DepartmentId,
                DepartmentName = p.Department != null ? p.Department.Name : string.Empty
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Guid> CreateAsync(CreatePositionDto dto)
    {
        if (!await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
            throw new InvalidOperationException("The specified department does not exist.");

        if (await _context.Positions.AnyAsync(p => p.Code == dto.Code))
            throw new InvalidOperationException($"Position code '{dto.Code}' already exists.");

        var position = new Position
        {
            Code = dto.Code,
            Name = dto.Name,
            Description = dto.Description,
            DepartmentId = dto.DepartmentId,
            IsActive = true
        };

        _context.Positions.Add(position);
        await _context.SaveChangesAsync();

        return position.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdatePositionDto dto)
    {
        var position = await _context.Positions.FindAsync(id);

        if (position == null)
            return false;

        if (!await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
            throw new InvalidOperationException("The specified department does not exist.");

        if (position.Code != dto.Code &&
            await _context.Positions.AnyAsync(p => p.Code == dto.Code))
        {
            throw new InvalidOperationException($"Position code '{dto.Code}' already exists.");
        }

        position.Code = dto.Code;
        position.Name = dto.Name;
        position.Description = dto.Description;
        position.DepartmentId = dto.DepartmentId;
        position.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var position = await _context.Positions.FindAsync(id);

        if (position == null)
            return false;

        position.IsActive = false;
        position.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}