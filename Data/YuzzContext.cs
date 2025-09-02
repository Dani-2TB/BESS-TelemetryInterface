using Microsoft.EntityFrameworkCore;
using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class YuzzContext : DbContext
    {
        
        public YuzzContext(DbContextOptions options): base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Module> Modules { get; set;}
        public DbSet<ConfigBess> ConfigBess { get; set;}

    }
}
