using DotnetAPI.Data;
using DotnetAPI.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace DotnetAPI.Models;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        // Resolve Services via DI
        var context = serviceProvider.GetRequiredService<YuzzContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Seed Roles
        string[] roleNames = { "Admin", "Operator" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }

        // Seed Admin User
        var adminEmail = Env.GetString("YUZZ_EMAIL");
        if (string.IsNullOrEmpty(adminEmail)) return; // Fail silent or throw based on preference

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var adminPassword = Env.GetString("YUZZ_PASSWORD") 
                ?? throw new Exception("YUZZ_PASSWORD missing");
            var adminUsername = Env.GetString("YUZZ_USERNAME") 
                ?? throw new Exception("YUZZ_USERNAME missing");

            // Fixed RUT generator usage
            var (rut, dv) = GenerateValidRut(11111111); 

            var newAdmin = new AppUser
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true,
                Rut = rut,
                Dv = dv,
                NombreCompleto = "System Administrator",
                Cargo = "Root"
            };

            var result = await userManager.CreateAsync(newAdmin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }

        // Seed Business Data (BESS)
        if (!context.Besses.Any())
        {
            var onGrid = new OperationMode { Name = "on_grid" };
            var offGrid = new OperationMode { Name = "off_grid" };

            context.OperationModes.AddRange(onGrid, offGrid);

            context.PcsModels.Add(new PcsModel
            {
                Name = "InfyPower BEG1K075G",
                RatedPower = 22000,
                VoltageMinDc = 150000,
                VoltageMaxDc = 1000000,
                CurrentMaxDc = 80000,
            });

            context.Besses.Add(new Bess
            {
                Name = "Prototype Config",
                CurrentMaxAcIn = 15000,
                CurrentMaxAcOut = 30000,
                OperationMode = offGrid
            });

            await context.SaveChangesAsync();
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