using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtlasERP.Infrastructure.Data.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("Positions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(500);

        builder.HasOne(x => x.Department)
               .WithMany(d => d.Positions)
               .HasForeignKey(x => x.DepartmentId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}