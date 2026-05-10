using System;
using System.Collections.Generic;
using CaRP.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CaRP.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Servicing> Servicings { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<WorkRegistration> WorkRegistrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.0.233;Database=carp;Username=post;Password=post");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Servicing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("servicing_pkey");

            entity.ToTable("servicing");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClerkUsername)
                .HasMaxLength(255)
                .HasColumnName("clerk_username");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.IssueDescription).HasColumnName("issue_description");
            entity.Property(e => e.MechanicName)
                .HasMaxLength(255)
                .HasColumnName("mechanic_name");
            entity.Property(e => e.ServiceDate).HasColumnName("service_date");
            entity.Property(e => e.ServiceNumber)
                .HasMaxLength(255)
                .HasColumnName("service_number");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Servicings)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("servicing_vehicle_id_fkey");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vehicles_pkey");

            entity.ToTable("vehicles");

            entity.HasIndex(e => e.RegistrationNumber, "vehicles_registration_number_key").IsUnique();

            entity.HasIndex(e => e.Vin, "vehicles_vin_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvailableFrom).HasColumnName("available_from");
            entity.Property(e => e.AvailableTo).HasColumnName("available_to");
            entity.Property(e => e.IsOwnedByCompany).HasColumnName("is_owned_by_company");
            entity.Property(e => e.RegistrationNumber)
                .HasMaxLength(20)
                .HasColumnName("registration_number");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(50)
                .HasColumnName("vehicle_type");
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .HasColumnName("vin");
        });

        modelBuilder.Entity<WorkRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("work_registrations_pkey");

            entity.ToTable("work_registrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClerkUsername)
                .HasMaxLength(255)
                .HasColumnName("clerk_username");
            entity.Property(e => e.CostPerHour)
                .HasPrecision(10, 2)
                .HasColumnName("cost_per_hour");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DurationHours)
                .HasPrecision(5, 2)
                .HasColumnName("duration_hours");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            entity.Property(e => e.WorkDate).HasColumnName("work_date");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.WorkRegistrations)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("work_registrations_vehicle_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
