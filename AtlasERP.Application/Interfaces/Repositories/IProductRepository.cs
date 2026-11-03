using System;
using System.Collections.Generic;
using System.Text;

using AtlasERP.Application.DTOs.Product;

namespace AtlasERP.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetAllAsync(bool includeInactive = false);
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateProductDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteAsync(Guid id);
}
