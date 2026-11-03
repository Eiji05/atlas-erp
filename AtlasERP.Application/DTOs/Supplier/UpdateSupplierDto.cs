using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Supplier;

public class UpdateSupplierDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    // No IsActive, no SupplierCode — same reasoning as every prior module
}