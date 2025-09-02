using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using DotnetAPI.Models;
using DotnetAPI.Data;

public static class ModuleService
{
    public static void AddModulesEndpoints(this IEndpointRouteBuilder app)
    {
        var bess = app.MapGroup("/modules");

        bess.MapGet("/", async (YuzzContext db) =>
            await db.Modules.ToListAsync());
        
        bess.MapPost("/", async (Module module, YuzzContext db) => 
        {
            db.Modules.Add(module);
            await db.SaveChangesAsync();
            return Results.Created($"/modules/{module.Id}", module);
        });
    }
}