using System;
using System.Collections.Generic;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EmployeeApp.Data.Data
{
    public partial class EmployeeDB2Context : DbContext
    {
        public EmployeeDB2Context()
        {
        }

        public EmployeeDB2Context(DbContextOptions<EmployeeDB2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-49J7JJ2\\SQLEXPRESS;Database=EmployeeDB2;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate")
                    .HasDefaultValueSql("(getdate())");


                entity.Property(e => e.LastModified).
                HasColumnType("datetime")
                .HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.CreatedPersonId).HasColumnName("createdPersonId");
                entity.Property(e => e.LastModifiedPersonId).HasColumnName("LastModifiedPersonId");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).HasColumnName("ID");

                

                entity.Property(e => e.CreatedPersonId).HasColumnName("createdPersonId");

                
                entity.Property(e => e.LastModifiedPersonId).HasColumnName("LastModifiedPersonId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate")
                    .HasDefaultValueSql("(getdate())");


                entity.Property(e => e.LastModified).
                HasColumnType("datetime")
                .HasDefaultValueSql("(sysdatetime())");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                

                entity.Property(e => e.CreatedPersonId).HasColumnName("createdPersonId");

                entity.Property(e => e.LastModifiedPersonId).HasColumnName("LastModifiedPersonId");
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate")
                    .HasDefaultValueSql("(getdate())");


                entity.Property(e => e.LastModified).
                HasColumnType("datetime")
                .HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Role1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.AddressId, "UQ__User__091C2A1A44CF3A5C")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedPersonId)
                .HasColumnName("createdPersonId")
                .HasDefaultValueSql("1");

                entity.Property(e => e.DateOfJoining).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.IsActive).HasColumnName("isActive")
                .HasDefaultValueSql("1");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(sysdatetime())");
                entity.Property(e => e.LastModifiedPersonId).HasColumnName("LastModifiedPersonId");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Address");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_User_Group")
                    .IsRequired(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
