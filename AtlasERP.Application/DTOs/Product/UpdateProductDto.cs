using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Product;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    // No IsActive — same lesson as Employee/Department: deactivation only via DELETE
    // No ProductCode — treating it as immutable, same reasoning as EmployeeNumber
}