using AtlasERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public Guid BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}
