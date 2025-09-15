using Microsoft.EntityFrameworkCore;
using DotnetAPI.Models; // Module, ConfigBess
//using AuthApi.Models;   // AppUser

namespace DotnetAPI.Data;

public class YuzzContext : DbContext
{
    public YuzzContext(DbContextOptions<YuzzContext> options) : base(options) { }

    // DbSets de tus modelos anteriores
    public DbSet<Module> Modules { get; set; } = default!;
    public DbSet<ConfigBess> ConfigBess { get; set; } = default!;
    
    // Usuarios para autenticación
    public DbSet<AppUser> Users { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Reglas de unicidad para AppUser
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}

