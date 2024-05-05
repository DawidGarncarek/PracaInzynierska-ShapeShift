using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAppDB.Data.Calories;

public partial class ShapeShiftCaloriesContext : DbContext
{
    public ShapeShiftCaloriesContext(DbContextOptions<ShapeShiftCaloriesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FoodProducts> FoodProducts { get; set; }

    public virtual DbSet<UserCaloriesNeeded> UserCaloriesNeeded { get; set; }

    public virtual DbSet<UserCaloriesResult> UserCaloriesResult { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodProducts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FoodProd__3214EC0796EFC921");

            entity.Property(e => e.Calories100g)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("Calories(100g)");
            entity.Property(e => e.Carbohydrates).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Fats).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Protein).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<UserCaloriesNeeded>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserCalo__3214EC0742ED16EF");

            entity.Property(e => e.CaloriesNeededResult).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NewCaloriesDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(250);
        });

        modelBuilder.Entity<UserCaloriesResult>(entity =>
        {
            entity.Property(e => e.Calories).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NewCaloriesDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.UserCaloriesResult)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_UserCaloriesResult_FoodProducts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}