using Microsoft.AspNetCore.Routing;
using DotnetAPI.Models;
using DotnetAPI.Data;

namespace Services;

public static class BessService
{
    public static void AddBessEndpoints(this IEndpointRouteBuilder app)
    {
        var bess = app.MapGroup("/bess");

        bess.MapGet("/", async (YuzzContext db) =>
            await db.Bess.ToListAsync());
        
        bess.MapPost("/", async (Bess bess, YuzzContext db) => 
        {
            await db.Bess.Add()
            return Results.Created($"/bess/{bess.Id}", bess);
        });
    }
}