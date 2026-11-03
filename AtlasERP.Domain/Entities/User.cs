using AtlasERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AtlasERP.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}
