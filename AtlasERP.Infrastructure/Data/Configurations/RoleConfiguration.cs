using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtlasERP.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public static readonly Guid AdministratorRoleId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static readonly Guid ManagerRoleId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static readonly Guid EmployeeRoleId =
        Guid.Parse("33333333-3333-3333-3333-333333333333");

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasData(
            new Role
            {
                Id = AdministratorRoleId,
                Name = "Administrator",
                Description = "System Administrator"
            },
            new Role
            {
                Id = ManagerRoleId,
                Name = "Manager",
                Description = "Department Manager"
            },
            new Role
            {
                Id = EmployeeRoleId,
                Name = "Employee",
                Description = "Regular Employee"
            }
        );
    }
}