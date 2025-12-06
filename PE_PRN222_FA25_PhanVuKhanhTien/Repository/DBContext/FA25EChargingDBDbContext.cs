using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Repository.DBContext;

public partial class FA25EChargingDBDbContext : DbContext
{
    public FA25EChargingDBDbContext()
    {
    }

    public FA25EChargingDBDbContext(DbContextOptions<FA25EChargingDBDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChargingSession> ChargingSessions { get; set; }

    public virtual DbSet<ChargingStation> ChargingStations { get; set; }

    public virtual DbSet<SystemUser> SystemUsers { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(local);Database=FA25EChargingDB;User Id=sa;Password=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChargingSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Charging__C9F492708F13CFB9");

            entity.ToTable("ChargingSession");

            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.KwhConsumed)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("KWhConsumed");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.StationId).HasColumnName("StationID");

            entity.HasOne(d => d.Driver).WithMany(p => p.ChargingSessions)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChargingS__Drive__3E52440B");

            entity.HasOne(d => d.Station).WithMany(p => p.ChargingSessions)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChargingS__Stati__3D5E1FD2");
        });

        modelBuilder.Entity<ChargingStation>(entity =>
        {
            entity.HasKey(e => e.StationId).HasName("PK__Charging__E0D8A6DDBB7A5B74");

            entity.ToTable("ChargingStation");

            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.MaxPower).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StationName).HasMaxLength(100);
        });

        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__SystemUs__1788CCAC08D7A45E");

            entity.ToTable("SystemUser");

            entity.HasIndex(e => e.Username, "UQ__SystemUs__536C85E4AC9F4A00").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserPassword).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
