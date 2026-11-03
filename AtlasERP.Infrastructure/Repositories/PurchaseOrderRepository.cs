using AtlasERP.Application.DTOs.PurchaseOrder;
using AtlasERP.Application.Interfaces.Repositories;
using AtlasERP.Domain.Entities;
using AtlasERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AtlasERP.Infrastructure.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly AtlasDbContext _context;

    // The only allowed next statuses for each current status.
    // An empty array means that status is terminal — nothing can follow it.
    private static readonly Dictionary<string, string[]> Transitions = new()
    {
        ["Draft"] = new[] { "PendingApproval", "Cancelled" },
        ["PendingApproval"] = new[] { "Approved", "Draft", "Cancelled" },
        ["Approved"] = new[] { "Ordered", "Cancelled" },
        ["Ordered"] = new[] { "PartiallyReceived", "Received", "Cancelled" },
        ["PartiallyReceived"] = new[] { "Received" },
        ["Received"] = new[] { "Closed" },
        ["Closed"] = Array.Empty<string>(),
        ["Cancelled"] = Array.Empty<string>()
    };

    public PurchaseOrderRepository(AtlasDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetAllAsync()
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Items)
                .ThenInclude(i => i.Product)
            .Select(po => MapToDto(po))
            .ToListAsync();
    }

    public async Task<PurchaseOrderDto?> GetByIdAsync(Guid id)
    {
        var po = await _context.PurchaseOrders
            .Include(x => x.Supplier)
            .Include(x => x.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

        return po == null ? null : MapToDto(po);
    }

    public async Task<Guid> CreateAsync(CreatePurchaseOrderDto dto)
    {
        if (!await _context.Suppliers.AnyAsync(s => s.Id == dto.SupplierId))
            throw new InvalidOperationException("The specified supplier does not exist.");

        if (dto.Items == null || dto.Items.Count == 0)
            throw new InvalidOperationException("A purchase order must have at least one item.");

        var productIds = dto.Items.Select(i => i.ProductId).ToList();

        if (productIds.Distinct().Count() != productIds.Count)
            throw new InvalidOperationException("Duplicate products are not allowed in the same order.");

        var existingProductCount = await _context.Products
            .CountAsync(p => productIds.Contains(p.Id));

        if (existingProductCount != productIds.Count)
            throw new InvalidOperationException("One or more specified products do not exist.");

        var purchaseOrder = new PurchaseOrder
        {
            PurchaseOrderNumber = GeneratePurchaseOrderNumber(),
            SupplierId = dto.SupplierId,
            OrderDate = dto.OrderDate,
            Status = "Draft",
            Items = dto.Items.Select(i => new PurchaseOrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _context.PurchaseOrders.Add(purchaseOrder);
        await _context.SaveChangesAsync();

        return purchaseOrder.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdatePurchaseOrderDto dto)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (purchaseOrder == null)
            return false;

        if (purchaseOrder.Status != "Draft")
            throw new InvalidOperationException("Only orders in Draft status can be edited.");

        if (!await _context.Suppliers.AnyAsync(s => s.Id == dto.SupplierId))
            throw new InvalidOperationException("The specified supplier does not exist.");

        if (dto.Items == null || dto.Items.Count == 0)
            throw new InvalidOperationException("A purchase order must have at least one item.");

        var productIds = dto.Items.Select(i => i.ProductId).ToList();

        if (productIds.Distinct().Count() != productIds.Count)
            throw new InvalidOperationException("Duplicate products are not allowed in the same order.");

        var existingProductCount = await _context.Products
            .CountAsync(p => productIds.Contains(p.Id));

        if (existingProductCount != productIds.Count)
            throw new InvalidOperationException("One or more specified products do not exist.");

        purchaseOrder.SupplierId = dto.SupplierId;
        purchaseOrder.OrderDate = dto.OrderDate;
        purchaseOrder.UpdatedAt = DateTime.UtcNow;

        // Replace all items: remove the old set, add the new set.
        // Safe here because we already confirmed Status == "Draft" —
        // a Draft order has no ReceivedQuantity history worth preserving.
        _context.PurchaseOrderItems.RemoveRange(purchaseOrder.Items);
        purchaseOrder.Items = dto.Items.Select(i => new PurchaseOrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList();

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangeStatusAsync(Guid id, string newStatus)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);

        if (purchaseOrder == null)
            return false;

        if (!Transitions.TryGetValue(purchaseOrder.Status, out var allowedNext) ||
            !allowedNext.Contains(newStatus))
        {
            throw new InvalidOperationException(
                $"Cannot change status from '{purchaseOrder.Status}' to '{newStatus}'.");
        }

        purchaseOrder.Status = newStatus;
        purchaseOrder.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static string GeneratePurchaseOrderNumber()
    {
        return $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }

    private static PurchaseOrderDto MapToDto(PurchaseOrder po)
    {
        return new PurchaseOrderDto
        {
            Id = po.Id,
            PurchaseOrderNumber = po.PurchaseOrderNumber,
            SupplierId = po.SupplierId,
            SupplierName = po.Supplier != null ? po.Supplier.CompanyName : string.Empty,
            OrderDate = po.OrderDate,
            Status = po.Status,
            TotalAmount = po.Items.Sum(i => i.Total),
            Items = po.Items.Select(i => new PurchaseOrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product != null ? i.Product.Name : string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Total = i.Total,
                ReceivedQuantity = i.ReceivedQuantity,
                RemainingQuantity = i.RemainingQuantity
            }).ToList()
        };
    }
}