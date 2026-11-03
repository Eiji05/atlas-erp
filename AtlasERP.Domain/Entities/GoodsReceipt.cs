using AtlasERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Domain.Entities
{
    public class GoodsReceipt : BaseEntity
    {
        public string ReceiptNumber { get; set; } = string.Empty;

        public Guid PurchaseOrderId { get; set; }

        public PurchaseOrder? PurchaseOrder { get; set; }

        public DateTime ReceivedDate { get; set; }

        public string Remarks { get; set; } = string.Empty;
    }
}
