using System;
using System.Collections.Generic;
using System.Text;

using AtlasERP.Application.DTOs.Position;

namespace AtlasERP.Application.Interfaces.Repositories;

public interface IPositionRepository
{
    Task<IEnumerable<PositionDto>> GetAllAsync(bool includeInactive = false);
    Task<PositionDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreatePositionDto dto);
    Task<bool> UpdateAsync(Guid id, UpdatePositionDto dto);
    Task<bool> DeleteAsync(Guid id);
}
