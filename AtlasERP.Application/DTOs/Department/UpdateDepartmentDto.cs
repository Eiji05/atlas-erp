using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Department;

public class UpdateDepartmentDto
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}