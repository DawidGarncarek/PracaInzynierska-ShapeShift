using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAppDB.Data.ContactForm;

public partial class ShapeShiftFormContext : DbContext
{
    public ShapeShiftFormContext(DbContextOptions<ShapeShiftFormContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContactMessage> ContactMessage { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactM__3214EC0759111594");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EMail)
                .HasMaxLength(250)
                .HasColumnName("E-mail");
            entity.Property(e => e.Message).HasMaxLength(350);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Topic).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}