using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAppDB.Data.Chat;

public partial class ShapeShiftChatContext : DbContext
{
    public ShapeShiftChatContext(DbContextOptions<ShapeShiftChatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserChatMessages> UserChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserChatMessages>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserChat__3214EC072D03CF78");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.MessageText).IsRequired();
            entity.Property(e => e.UserName).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}