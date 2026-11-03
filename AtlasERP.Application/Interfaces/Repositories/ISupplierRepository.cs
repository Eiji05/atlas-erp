using System;
using System.Collections.Generic;
using System.Text;

using AtlasERP.Application.DTOs.Supplier;

namespace AtlasERP.Application.Interfaces.Repositories;

public interface ISupplierRepository
{
    Task<IEnumerable<SupplierDto>> GetAllAsync(bool includeInactive = false);
    Task<SupplierDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateSupplierDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateSupplierDto dto);
    Task<bool> DeleteAsync(Guid id);
}