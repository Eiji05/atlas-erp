using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Position;

public class CreatePositionDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}