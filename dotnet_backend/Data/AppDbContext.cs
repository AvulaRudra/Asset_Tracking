using Microsoft.EntityFrameworkCore;
using dotnet_backend.Entities;

namespace dotnet_backend.Data;

public class AppDbContext : DbContext
{
public DbSet<User> Users { get; set; } = null!;
    public DbSet<LocalUser> LocalUsers { get; set; } = null!;
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<AssetTracking> AssetTrackings { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Users table (composite PK id+provider)
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Provider });
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.Provider).HasMaxLength(32);
            entity.Property(e => e.Email).HasMaxLength(320);
        });

        // LocalUsers table
        modelBuilder.Entity<LocalUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.HasIndex(e => e.Email).IsUnique().HasFilter("[Email] IS NOT NULL");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
        });

        // Asset Tracking relationship
        modelBuilder.Entity<AssetTracking>()
            .HasOne(t => t.Asset)
            .WithMany(a => a.Trackings)
            .HasForeignKey(t => t.AssetId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.HasMany(a => a.Trackings)
                  .WithOne(t => t.Asset)
                  .HasForeignKey(t => t.AssetId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}

