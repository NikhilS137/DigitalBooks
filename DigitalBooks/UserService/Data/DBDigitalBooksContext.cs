using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UserService.Models;

namespace UserService.Data
{
    public partial class DBDigitalBooksContext : DbContext
    {
        public DBDigitalBooksContext()
        {
        }

        public DBDigitalBooksContext(DbContextOptions<DBDigitalBooksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookMaster> BookMasters { get; set; } = null!;
        public virtual DbSet<CategoryMaster> CategoryMasters { get; set; } = null!;
        public virtual DbSet<Purchase> Purchases { get; set; } = null!;
        public virtual DbSet<RoleMaster> RoleMasters { get; set; } = null!;
        public virtual DbSet<UserMaster> UserMasters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DBDigitalBooks;Trusted_Connection=True;");
                optionsBuilder.UseSqlServer("Server=tcp:1sqlserver.database.windows.net,1433;Initial Catalog=DigitalBooks;Persist Security Info=False;User ID=nikhil;Password=sql@12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookMaster>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.BookMasters)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookMaster_CategoryMaster");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookMasters)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookMaster_UserMaster");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.IsRefunded).IsFixedLength();

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchase_BookMaster");
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserMasters)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMaster_RoleMaster");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
