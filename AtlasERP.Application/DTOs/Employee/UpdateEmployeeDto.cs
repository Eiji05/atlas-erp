using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Employee;

public class UpdateEmployeeDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid PositionId { get; set; }
}