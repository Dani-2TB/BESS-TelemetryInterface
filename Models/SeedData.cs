using DotnetAPI.Data;
using DotnetAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DotnetAPI.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var contextOptions = serviceProvider.GetRequiredService<DbContextOptions<YuzzContext>>();
        
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // 1. Seed Roles
        string[] roleNames = { "Admin", "Operator" };
        foreach (var roleName in roleNames)
        {
            var roleExist = roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult();
            if (!roleExist)
            {
                roleManager.CreateAsync(new IdentityRole<Guid>(roleName)).GetAwaiter().GetResult();
            }
        }

        // 2. Seed Admin User
        var adminEmail = DotNetEnv.Env.GetString("YUZZ_EMAIL"); 
        var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();

        if (adminUser == null)
        {
            var adminPassword = DotNetEnv.Env.GetString("YUZZ_PASSWORD");
            var adminUsername = DotNetEnv.Env.GetString("YUZZ_USERNAME");
            
            if (string.IsNullOrEmpty(adminPassword))
            {
                throw new Exception("YUZZ_PASSWORD is missing in .env file");
            }

            if (string.IsNullOrEmpty(adminUsername))
            {
                throw new Exception("YUZZ_USERNAME is missing in .env file");
            }

            if (string.IsNullOrEmpty(adminEmail))
            {
                throw new Exception("YUZZ_EMAIL is missing in .env file");
            }

            var (rut, dv) = GenerateValidRut(11111111); // Get DV from rut "11.111.111"

            var newAdmin = new AppUser
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true,
                Rut = rut,
                Dv = dv,
                NombreCompleto = "System Administrator",
                Cargo = "System Administrator"
            };

            var createAdmin = userManager.CreateAsync(newAdmin, adminPassword).GetAwaiter().GetResult();

            if (createAdmin.Succeeded)
            {
                userManager.AddToRoleAsync(newAdmin, "Admin").GetAwaiter().GetResult();
            }
        }

        // 3. Seed Business Data
        using (var context = new YuzzContext(contextOptions))
        {
            if (context is null ||
                context.Besses is null ||
                context.OperationModes is null ||
                context.PcsModels is null)
            {
                throw new ArgumentNullException("Null Database or Entity Context");
            }

            if (context.Besses.Any() ||
                context.OperationModes.Any() ||
                context.PcsModels.Any())
            {
                return; 
            }

            var onGrid = new OperationMode { Name = "on_grid" };
            var offGrid = new OperationMode { Name = "off_grid" };

            context.OperationModes.AddRange(onGrid, offGrid);

            context.PcsModels.Add(
                new PcsModel
                {
                    Name = "InfyPower BEG1K075G",
                    RatedPower = 22000,
                    VoltageMinDc = 150000,
                    VoltageMaxDc = 1000000,
                    CurrentMaxDc = 80000,
                }
            );

            context.Besses.Add(
                new Bess
                {
                    Name = "Prototype Config",
                    CurrentMaxAcIn = 15000,
                    CurrentMaxAcOut = 30000,
                    OperationMode = offGrid
                }
            );

            context.SaveChanges();
        }
    }

    private static (int rut, string dv) GenerateValidRut(int rut)
    {
        int sum = 0;
        int multiplier = 2;
        int tempRut = rut;

        while (tempRut != 0)
        {
            sum += (tempRut % 10) * multiplier;
            tempRut /= 10;
            multiplier++;
            if (multiplier == 8) multiplier = 2;
        }

        int remainder = 11 - (sum % 11);
        string dv = remainder == 11 ? "0" : remainder == 10 ? "K" : remainder.ToString();

        return (rut, dv);
    }
}