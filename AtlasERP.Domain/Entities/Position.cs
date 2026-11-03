using AtlasERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasERP.Domain.Entities;

public class Position : BaseEntity
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}