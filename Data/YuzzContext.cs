using Microsoft.EntityFrameworkCore;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Data;

public class YuzzContext : DbContext
{
    public YuzzContext(DbContextOptions<YuzzContext> options) : base(options) { }

    public DbSet<OperationMode> OperationModes { get; set; }
    public DbSet<Bess> Besses {get; set;}
    public DbSet<Battery> Batteries {get; set;}
    public DbSet<PcsModel> PcsModels {get; set;}
    public DbSet<Pcs> Pcs {get; set;}
    public DbSet<AppUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Bess -> OperationMode
        modelBuilder.Entity<Bess>()
            .HasOne(b => b.OperationMode)
            .WithMany()
            .HasForeignKey(b => b.OperationModeId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("OPERATION_MODE_BESS");

        // Battery -> Bess
        modelBuilder.Entity<Battery>()
            .HasOne(b => b.Bess)
            .WithMany()
            .HasForeignKey(b => b.BessId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("BESS_BATTERY");

        // Pcs -> Battery
        modelBuilder.Entity<Pcs>()
            .HasOne(p => p.Battery)
            .WithMany()
            .HasForeignKey(p => p.BatteryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("BATTERY_PCS");

        // Pcs -> PcsModel
        modelBuilder.Entity<Pcs>()
            .HasOne(p => p.PcsModel)
            .WithMany()
            .HasForeignKey(p => p.PcsModelId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("PCS_MODEL");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}

