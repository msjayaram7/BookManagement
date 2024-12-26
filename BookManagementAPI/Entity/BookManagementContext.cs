using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookManagementAPI.Entity;

public partial class BookManagementContext : DbContext
{
    private readonly IConfiguration _config;
    public BookManagementContext(IConfiguration config)
    {
        this._config = config;
    }

    public BookManagementContext(DbContextOptions<BookManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookDetail> BookDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DbConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookDetail>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__BookDeta__3DE0C227E33D5ACC");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Author)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedTimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountPercentage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Genre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.PublishedYear)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ratings)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedTimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
