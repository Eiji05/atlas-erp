using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Product;

public class ProductDto
{
    public Guid Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }

    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public Guid BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
}
