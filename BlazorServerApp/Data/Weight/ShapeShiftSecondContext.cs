using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAppDB.Data.Weight;

public partial class ShapeShiftSecondContext : DbContext
{
    public ShapeShiftSecondContext(DbContextOptions<ShapeShiftSecondContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Weight> Weight { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Weight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Weight__3214EC07FCA0EF7C");

            entity.Property(e => e.GoalWeight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserName).HasMaxLength(150);
            entity.Property(e => e.UserWeight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.WeightDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}