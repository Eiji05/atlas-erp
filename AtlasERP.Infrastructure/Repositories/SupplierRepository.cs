using System;
using System.Collections.Generic;
using System.Text;

using AtlasERP.Application.DTOs.Supplier;
using AtlasERP.Application.Interfaces.Repositories;
using AtlasERP.Domain.Entities;
using AtlasERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AtlasERP.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AtlasDbContext _context;

    public SupplierRepository(AtlasDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync(bool includeInactive = false)
    {
        var query = _context.Suppliers.AsQueryable();

        if (!includeInactive)
            query = query.Where(s => s.IsActive);

        return await query
            .Select(s => new SupplierDto
            {
                Id = s.Id,
                SupplierCode = s.SupplierCode,
                CompanyName = s.CompanyName,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                IsActive = s.IsActive
            })
            .ToListAsync();
    }

    public async Task<SupplierDto?> GetByIdAsync(Guid id)
    {
        return await _context.Suppliers
            .Where(s => s.Id == id)
            .Select(s => new SupplierDto
            {
                Id = s.Id,
                SupplierCode = s.SupplierCode,
                CompanyName = s.CompanyName,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                IsActive = s.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Guid> CreateAsync(CreateSupplierDto dto)
    {
        if (await _context.Suppliers.AnyAsync(s => s.SupplierCode == dto.SupplierCode))
            throw new InvalidOperationException($"Supplier code '{dto.SupplierCode}' already exists.");

        if (await _context.Suppliers.AnyAsync(s => s.Email == dto.Email))
            throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");

        var supplier = new Supplier
        {
            SupplierCode = dto.SupplierCode,
            CompanyName = dto.CompanyName,
            ContactPerson = dto.ContactPerson,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            IsActive = true
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return supplier.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateSupplierDto dto)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
            return false;

        if (supplier.Email != dto.Email &&
            await _context.Suppliers.AnyAsync(s => s.Email == dto.Email))
        {
            throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");
        }

        supplier.CompanyName = dto.CompanyName;
        supplier.ContactPerson = dto.ContactPerson;
        supplier.Email = dto.Email;
        supplier.PhoneNumber = dto.PhoneNumber;
        supplier.Address = dto.Address;
        supplier.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
            return false;

        supplier.IsActive = false;
        supplier.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
