using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Position;

public class UpdatePositionDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    // No IsActive here — same lesson from Employee: deactivation only via DELETE
}