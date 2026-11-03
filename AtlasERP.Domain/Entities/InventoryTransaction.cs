using AtlasERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Domain.Entities
{
    public class InventoryTransaction : BaseEntity
    {
        public Guid ProductId { get; set; }

        public Product? Product { get; set; }

        public int Quantity { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public string ReferenceNumber { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; }
    }
}
