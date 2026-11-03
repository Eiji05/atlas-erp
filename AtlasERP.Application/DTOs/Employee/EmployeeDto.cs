using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Application.DTOs.Employee;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public DateOnly HireDate { get; set; }
    public decimal? Salary { get; set; }   // nullable so the controller can strip it
    public bool IsActive { get; set; }

    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public Guid PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
}
