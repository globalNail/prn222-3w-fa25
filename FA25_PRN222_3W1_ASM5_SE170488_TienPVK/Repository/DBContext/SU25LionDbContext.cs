using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Repository.DBContext;

public partial class SU25LionDbContext : DbContext
{
    public SU25LionDbContext()
    {
    }

    public SU25LionDbContext(DbContextOptions<SU25LionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LionAccount> LionAccounts { get; set; }

    public virtual DbSet<LionProfile> LionProfiles { get; set; }

    public virtual DbSet<LionType> LionTypes { get; set; }


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
//        => optionsBuilder.UseSqlServer("Server=(local);Database=SU25LionDB;User Id=sa;Password=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LionAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("LionAccount");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<LionProfile>(entity =>
        {
            entity.ToTable("LionProfile");

            entity.Property(e => e.Characteristics).HasMaxLength(2000);
            entity.Property(e => e.LionName).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Warning).HasMaxLength(1500);

            entity.HasOne(d => d.LionType).WithMany(p => p.LionProfiles)
                .HasForeignKey(d => d.LionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LionProfile_LionType");
        });

        modelBuilder.Entity<LionType>(entity =>
        {
            entity.ToTable("LionType");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.LionTypeName).HasMaxLength(250);
            entity.Property(e => e.Origin).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
