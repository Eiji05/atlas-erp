using AtlasERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
