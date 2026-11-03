using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Product;

public class CreateProductDto
{
    public string ProductCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
}