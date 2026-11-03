using System;

namespace AtlasERP.Application.DTOs.PurchaseOrder;

public class UpdatePurchaseOrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}