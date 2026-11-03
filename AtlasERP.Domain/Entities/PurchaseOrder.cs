using AtlasERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Domain.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public Guid SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Draft";   // changed default from "Pending"

        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}
