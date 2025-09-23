
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

public static class ConfigApiEndpoints
{
    // public static void AddBessEndpoints(this IEndpointRouteBuilder app)
    // {
    //     var bessConfig = app.MapGroup("/bess");

    //     bessConfig.MapGet("/", async (YuzzContext db) => await db.Besses.ToListAsync());
        
    //     bessConfig.MapPost("/", async (Bess bess, int bessId, YuzzContext db) => 
    //     {
    //         Bess? module = await db.Modules.FindAsync(moduleId);

    //         if (module is null)
    //             return Results.BadRequest("There is no module with this id.");
            
    //         config.Module = module;
    //         config.ModuleId = module.Id;

    //         db.ConfigBess.Add(config);

    //         await db.SaveChangesAsync();
    //         return Results.Created($"/bessConfig/{config.Id}", config);
    //     });

    //     bessConfig.MapGet("/{id}", async (Guid id, YuzzContext db) => 
    //     {
    //         var config = await db.ConfigBess.FindAsync(id);
    //         return config is not null ? Results.Ok(config) : Results.NotFound();
    //     });

    //     bessConfig.MapPut("/{id}", async (Guid id, ConfigBess updatedConfig, YuzzContext db) => 
    //     {
    //         var oldConfig = await db.ConfigBess.FindAsync(id);
    //         if (oldConfig is null) return Results.NotFound();

    //         oldConfig.MaxDCCurrent = updatedConfig.MaxDCCurrent;
    //         oldConfig.MinDCCurrent = updatedConfig.MinDCCurrent;

    //         await db.SaveChangesAsync();
    //         return Results.Accepted("Updated Module Succesfully");
    //     });

    //     bessConfig.MapDelete("/{id}", async (Guid id, YuzzContext db) => 
    //     {
    //         var config = await db.ConfigBess.FindAsync(id);
    //         if (config is null) return Results.NotFound();

    //         db.ConfigBess.Remove(config);
    //         await db.SaveChangesAsync();
    //         return Results.NoContent();
    //     });
//    }
}