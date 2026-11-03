using AtlasERP.Application.DTOs.PurchaseOrder;

namespace AtlasERP.Application.Interfaces.Repositories;

public interface IPurchaseOrderRepository
{
    Task<IEnumerable<PurchaseOrderDto>> GetAllAsync();
    Task<PurchaseOrderDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreatePurchaseOrderDto dto);
    Task<bool> UpdateAsync(Guid id, UpdatePurchaseOrderDto dto);
    Task<bool> ChangeStatusAsync(Guid id, string newStatus);
}