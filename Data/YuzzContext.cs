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
    public DbSet<Pcs> Pcs { get; set; }
    // public DbSet<AppUser> Users { get; set; }
    
    
    /* OnModelCreating Override
    * This function overrides Entity Framework's function when creating models
    * Here you can setup your procedures for configuring your models, first it
    * executes the parent function, and then you can use the ModelBuilder to
    * setup foreign keys, constraints, indexes, etc, along with any other
    * procedures you want while creating an entity.
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Add Bess -> OperationMode Foreign Key
        modelBuilder.Entity<Bess>()
            .HasOne(b => b.OperationMode)
            .WithMany()
            .HasForeignKey(b => b.OperationModeId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("OPERATION_MODE_BESS");

        // Add Battery -> Bess Foreign Key
        modelBuilder.Entity<Battery>()
            .HasOne(b => b.Bess)
            .WithMany()
            .HasForeignKey(b => b.BessId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("BESS_BATTERY");
        
        // Autoincrement modbus_id
        modelBuilder.Entity<Battery>()
            .Property(b => b.ModbusId)
            .ValueGeneratedOnAdd();

        // Add modbus_id unique field
        modelBuilder.Entity<Battery>()
            .HasIndex(b => b.ModbusId)
            .IsUnique();

        // Add Pcs -> Battery Foreign Key
        modelBuilder.Entity<Pcs>()
            .HasOne(p => p.Battery)
            .WithMany()
            .HasForeignKey(p => p.BatteryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("BATTERY_PCS");

        // Add Pcs -> PcsModel Foreign Key
        modelBuilder.Entity<Pcs>()
            .HasOne(p => p.PcsModel)
            .WithMany()
            .HasForeignKey(p => p.PcsModelId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("PCS_MODEL");

        // Autoincrement modbus_id
        modelBuilder.Entity<Pcs>()
            .Property(p => p.ModbusId)
            .ValueGeneratedOnAdd();

        // Agregar modbus_id editable
        modelBuilder.Entity<Pcs>()
            .HasIndex(p => p.ModbusId)
            .IsUnique();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configures the database context to use the Naming Conventions Plugin
        // and setup snake case so properties such as ModbusId turn into modbus_id
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}

