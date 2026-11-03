using System;
using System.Collections.Generic;
using System.Text;

using AtlasERP.Application.DTOs.Product;
using AtlasERP.Application.Interfaces.Repositories;
using AtlasERP.Domain.Entities;
using AtlasERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AtlasERP.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AtlasDbContext _context;

    public ProductRepository(AtlasDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(bool includeInactive = false)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .AsQueryable();

        if (!includeInactive)
            query = query.Where(p => p.IsActive);

        return await query
            .Select(p => new ProductDto
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                BrandId = p.BrandId,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                BrandId = p.BrandId,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Guid> CreateAsync(CreateProductDto dto)
    {
        if (!await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId))
            throw new InvalidOperationException("The specified category does not exist.");

        if (!await _context.Brands.AnyAsync(b => b.Id == dto.BrandId))
            throw new InvalidOperationException("The specified brand does not exist.");

        if (await _context.Products.AnyAsync(p => p.ProductCode == dto.ProductCode))
            throw new InvalidOperationException($"Product code '{dto.ProductCode}' already exists.");

        var product = new Product
        {
            ProductCode = dto.ProductCode,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId,
            IsActive = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        if (!await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId))
            throw new InvalidOperationException("The specified category does not exist.");

        if (!await _context.Brands.AnyAsync(b => b.Id == dto.BrandId))
            throw new InvalidOperationException("The specified brand does not exist.");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;
        product.BrandId = dto.BrandId;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}