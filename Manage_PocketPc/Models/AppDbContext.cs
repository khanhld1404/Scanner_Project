using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Manage_PocketPc.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IK> IKs { get; set; } = null!;
        public virtual DbSet<Keyence_version> Keyence_versions { get; set; } = null!;
        public virtual DbSet<LoginUser> LoginUsers { get; set; } = null!;
        public virtual DbSet<RFC> RFCs { get; set; } = null!;
        public virtual DbSet<ScanDatum> ScanData { get; set; } = null!;
        public virtual DbSet<UpdateHistory> UpdateHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Keyence_version>(entity =>
            {
                entity.HasKey(e => e.Version)
                    .HasName("PK__Keyence___0F5401357601EF6C");

                entity.Property(e => e.Update_time).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ScanDatum>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UpdateHistory>(entity =>
            {
                entity.Property(e => e.Version).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
