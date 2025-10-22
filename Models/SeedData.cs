using DotnetAPI.Data;
using DotnetAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new YuzzContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<YuzzContext>>()))
        {
            // Check for seeded items or database to exist
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
                return; // Database is seeded
            }

            var onGrid = new OperationMode { Name = "On Grid" };
            var offGrid = new OperationMode { Name = "Off Grid" };

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
}