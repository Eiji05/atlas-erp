using AtlasERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Domain.Entities
{
    public class GoodsReceiptItem : BaseEntity
    {
        public Guid GoodsReceiptId { get; set; }

        public GoodsReceipt? GoodsReceipt { get; set; }

        public Guid ProductId { get; set; }

        public Product? Product { get; set; }

        public int QuantityReceived { get; set; }
    }
}
