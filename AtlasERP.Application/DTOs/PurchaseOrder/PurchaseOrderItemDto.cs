using System;

namespace AtlasERP.Application.DTOs.PurchaseOrder;

public class PurchaseOrderItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public int ReceivedQuantity { get; set; }
    public int RemainingQuantity { get; set; }
}