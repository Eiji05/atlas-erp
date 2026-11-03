using System;
using System.Collections.Generic;

namespace AtlasERP.Application.DTOs.PurchaseOrder;

public class UpdatePurchaseOrderDto
{
    public Guid SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
}