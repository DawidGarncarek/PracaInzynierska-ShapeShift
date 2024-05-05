using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAppDB.Data.Exercises;

public partial class ShapeShiftExercisesContext : DbContext
{
    public ShapeShiftExercisesContext(DbContextOptions<ShapeShiftExercisesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exercises> Exercises { get; set; }

    public virtual DbSet<PossibleAnswersExercises> PossibleAnswersExercises { get; set; }

    public virtual DbSet<QuestionExercises> QuestionExercises { get; set; }

    public virtual DbSet<UserAnswersExercises> UserAnswersExercises { get; set; }

    public virtual DbSet<UserExerciseSets> UserExerciseSets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercises>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exercise__3214EC07213E22E3");

            entity.Property(e => e.DifficultyLevel).HasMaxLength(50);
            entity.Property(e => e.Goals).HasMaxLength(50);
            entity.Property(e => e.IntensityLevel).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<PossibleAnswersExercises>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Possible__3214EC07E130F9B6");

            entity.Property(e => e.AnswerOption)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AnswerText).HasMaxLength(500);

            entity.HasOne(d => d.Question).WithMany(p => p.PossibleAnswersExercises)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PossibleA__Quest__24285DB4");
        });

        modelBuilder.Entity<QuestionExercises>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC072175128F");

            entity.Property(e => e.Category).HasMaxLength(250);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.QuestionText).HasMaxLength(500);
        });

        modelBuilder.Entity<UserAnswersExercises>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAnsw__3214EC0791BC66AC");

            entity.Property(e => e.DateAnswered).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(150);

            entity.HasOne(d => d.ChosenAnswer).WithMany(p => p.UserAnswersExercises)
                .HasForeignKey(d => d.ChosenAnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAnswe__Chose__27F8EE98");

            entity.HasOne(d => d.Question).WithMany(p => p.UserAnswersExercises)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAnswe__Quest__2704CA5F");
        });

        modelBuilder.Entity<UserExerciseSets>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserExer__3214EC07D2F67A20");

            entity.Property(e => e.DateAssigned).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasOne(d => d.ExerciseSet).WithMany(p => p.UserExerciseSets)
                .HasForeignKey(d => d.ExerciseSetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserExerc__Exerc__308E3499");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}