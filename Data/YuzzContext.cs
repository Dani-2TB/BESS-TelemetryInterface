using Microsoft.EntityFrameworkCore;
using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    // Aquí defines la clase ApplicationDbContext
    public class YuzzContext : DbContext
    {
    
    public YuzzContext(DbContextOptions options): base(options) {}

    public DbSet<User> Users { get; set; }



    }
}
