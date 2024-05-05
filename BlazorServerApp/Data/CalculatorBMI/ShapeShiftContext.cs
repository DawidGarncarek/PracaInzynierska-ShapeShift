using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAppDB.Data.CalculatorBMI;

public partial class ShapeShiftContext : DbContext
{
    public ShapeShiftContext(DbContextOptions<ShapeShiftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BmiResult> BmiResult { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BmiResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BmiResul__3214EC0701F670CD");

            entity.Property(e => e.CalculationDate).HasColumnType("datetime");
            entity.Property(e => e.Result).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.UserName).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}