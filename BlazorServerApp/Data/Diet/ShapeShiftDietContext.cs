using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAppDB.Data.Diet;

public partial class ShapeShiftDietContext : DbContext
{
    public ShapeShiftDietContext(DbContextOptions<ShapeShiftDietContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Meals> Meals { get; set; }

    public virtual DbSet<PossibleAnswersDiet> PossibleAnswersDiet { get; set; }

    public virtual DbSet<QuestionDiet> QuestionDiet { get; set; }

    public virtual DbSet<Recipes> Recipes { get; set; }

    public virtual DbSet<UserAnswersDiet> UserAnswersDiet { get; set; }

    public virtual DbSet<UserAnswersDietCalories> UserAnswersDietCalories { get; set; }

    public virtual DbSet<UserMealSets> UserMealSets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meals>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Meals__3214EC070A1E96B8");

            entity.Property(e => e.Allergy).HasMaxLength(50);
            entity.Property(e => e.Carbohydrates).HasColumnType("decimal(4, 1)");
            entity.Property(e => e.ExcludedProducts).HasMaxLength(50);
            entity.Property(e => e.Fats).HasColumnType("decimal(4, 1)");
            entity.Property(e => e.FoodType).HasMaxLength(50);
            entity.Property(e => e.Goals).HasMaxLength(50);
            entity.Property(e => e.MealType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.ProductType).HasMaxLength(50);
            entity.Property(e => e.Protein).HasColumnType("decimal(4, 1)");
            entity.Property(e => e.RegionTypeFood).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<PossibleAnswersDiet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Possible__3214EC07D8DA89C0");

            entity.Property(e => e.AnswerOption)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AnswerText).HasMaxLength(500);

            entity.HasOne(d => d.Question).WithMany(p => p.PossibleAnswersDiet)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PossibleA__Quest__4B422AD5");
        });

        modelBuilder.Entity<QuestionDiet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07C9F24106");

            entity.Property(e => e.Category).HasMaxLength(250);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.QuestionText).HasMaxLength(500);
        });

        modelBuilder.Entity<Recipes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipes__3214EC07D7440062");

            entity.Property(e => e.RecipeText).IsRequired();

            entity.HasOne(d => d.Meal).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.MealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recipes__MealId__5C6CB6D7");
        });

        modelBuilder.Entity<UserAnswersDiet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAnsw__3214EC072CF50A74");

            entity.Property(e => e.DateAnswered).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(150);

            entity.HasOne(d => d.ChosenAnswer).WithMany(p => p.UserAnswersDiet)
                .HasForeignKey(d => d.ChosenAnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAnswe__Chose__4F12BBB9");

            entity.HasOne(d => d.Question).WithMany(p => p.UserAnswersDiet)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAnswe__Quest__4E1E9780");
        });

        modelBuilder.Entity<UserAnswersDietCalories>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAnsw__3214EC07C4914BC2");

            entity.Property(e => e.DateAnswered).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(150);

            entity.HasOne(d => d.Question).WithMany(p => p.UserAnswersDietCalories)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAnswe__Quest__51EF2864");
        });

        modelBuilder.Entity<UserMealSets>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMeal__3214EC072FC29B39");

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasOne(d => d.Breakfast).WithMany(p => p.UserMealSetsBreakfast)
                .HasForeignKey(d => d.BreakfastId)
                .HasConstraintName("FK__UserMealS__Break__5F492382");

            entity.HasOne(d => d.Dinner).WithMany(p => p.UserMealSetsDinner)
                .HasForeignKey(d => d.DinnerId)
                .HasConstraintName("FK__UserMealS__Dinne__61316BF4");

            entity.HasOne(d => d.Lunch).WithMany(p => p.UserMealSetsLunch)
                .HasForeignKey(d => d.LunchId)
                .HasConstraintName("FK__UserMealS__Lunch__603D47BB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}