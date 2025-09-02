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

        bess.MapGet("/{id}", async (Guid id, YuzzContext db) => 
        {
            var module = await db.Modules.FindAsync(id);
            return module is not null ? Results.Ok(module) : Results.NotFound();
        });

        bess.MapPut("/{id}", async (Guid id, Module updatedModule, YuzzContext db) => 
        {
            var module = await db.Modules.FindAsync(id);
            if (module is null) return Results.NotFound();

            module.Name = updatedModule.Name;


            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        bess.MapDelete("/{id}", async (Guid id, YuzzContext db) => 
        {
            var module = await db.Modules.FindAsync(id);
            if (module is null) return Results.NotFound();

            db.Modules.Remove(module);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}