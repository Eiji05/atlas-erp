using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Department;

public class DepartmentDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}