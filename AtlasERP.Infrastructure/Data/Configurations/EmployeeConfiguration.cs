using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtlasERP.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EmployeeNumber)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(x => x.FirstName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.LastName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Salary)
               .HasPrecision(18, 2);

        builder.HasOne(x => x.Department)
               .WithMany()
               .HasForeignKey(x => x.DepartmentId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Position)
               .WithMany(p => p.Employees)
               .HasForeignKey(x => x.PositionId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
