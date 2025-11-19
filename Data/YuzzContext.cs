using Microsoft.EntityFrameworkCore;
using DotnetAPI.Models.Domain;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DotnetAPI.Data;

public class YuzzContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public YuzzContext(DbContextOptions<YuzzContext> options) : base(options) { }

    public DbSet<OperationMode> OperationModes { get; set; }
    public DbSet<Bess> Besses {get; set;}
    public DbSet<Battery> Batteries {get; set;}
    public DbSet<PcsModel> PcsModels {get; set;}
    public DbSet<Pcs> Pcs { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
<<<<<<< HEAD
     public DbSet<AppUser> Users { get; set; }
    
=======
>>>>>>> 325a4b1 (feat: added identity auth, fix: soc validation)
    
    /* OnModelCreating Override
    * This function overrides Entity Framework's function when creating models
    * Here you can setup your procedures for configuring your models, first it
    * executes the parent function, and then you can use the ModelBuilder to
    * setup foreign keys, constraints, indexes, etc, along with any other
    * procedures you want while creating an entity.
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Identity related tables
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuditLog>()
            .Property(b => b.Timestamp)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        // Add Rut unique field for AppUser
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Rut)
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

        // Add editable can_id
        modelBuilder.Entity<Pcs>()
            .HasIndex(p => p.CanId)
            .IsUnique();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configures the database context to use the Naming Conventions Plugin
        // and setup snake case so properties such as ModbusId turn into modbus_id
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditEntries = CreateAuditLogEntries();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (auditEntries.Count > 0)
        {
            await AuditLogs.AddRangeAsync(auditEntries);
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }

    /* Create Audit Log Entries
    * This function is called by SaveChangesAsync to inspect Entity Framework's
    * ChangeTracker before a save operation. It identifies all Added, Modified,
    * and Deleted entities (excluding AuditLog entries themselves).
    * It then creates a new AuditLog object for each change, capturing the
    * entity type, primary key, and a JSON representation of the changes.
    * For 'Added', it serializes all current values.
    * For 'Deleted', it serializes all original values.
    * For 'Modified', it serializes only the properties that changed, 
    * including their Old and New values.
    * It returns a list of these AuditLog objects to be saved to the database.
    */
    private List<AuditLog> CreateAuditLogEntries()
    {
        var auditEntries = new List<AuditLog>();

        foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is not AuditLog && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)))
        {
            var log = new AuditLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.UtcNow,
                LogLevel = "Information", 
                Category = "DatabaseAudit",
                EntityType = entry.Entity.GetType().Name,
                EntityId = GetPrimaryKey(entry),
                Message = $"Entity '{entry.Entity.GetType().Name}' was {entry.State}."
            };

            var changes = new Dictionary<string, object>();

            if (entry.State == EntityState.Added)
            {
                log.Changes = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
            }
            else if (entry.State == EntityState.Deleted)
            {
                log.Changes = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
            }
            else if (entry.State == EntityState.Modified)
            {
                foreach (var prop in entry.Properties.Where(p => p.IsModified))
                {
                    changes[prop.Metadata.Name] = new
                    {
                        Old = prop.OriginalValue,
                        New = prop.CurrentValue
                    };
                }
                log.Changes = JsonSerializer.Serialize(changes);
            }

            auditEntries.Add(log);
        }

        return auditEntries;
    }
    
    private static string? GetPrimaryKey(EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        if (key != null)
        {
            var keyValues = key.Properties.Select(p => entry.Property(p.Name).CurrentValue);
            return string.Join(",", keyValues);
        }
        return null; 
    }
}