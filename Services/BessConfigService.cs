using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Services;

public static class ConfigApiEndpoints
{
    public static void MapConfigEndpoints(this WebApplication app)
    {
        MapBatteryEndpoints(app);
        MapPcsEndpoints(app);
        MapBessEndpoints(app);
    }

    private static void MapBatteryEndpoints(WebApplication app)
    {
        var batteryGroup = app.MapGroup("/api/batteries").RequireAuthorization("ApiAuth");


        // GET: all
        batteryGroup.MapGet("/", async (YuzzContext db) =>
        {
            var batteries = await db.Batteries
                .Include(b => b.Bess)
                .ThenInclude(b => b.OperationMode)
                .ToListAsync();
            return Results.Ok(batteries);
        });

        // GET: by id
        batteryGroup.MapGet("/{id:int}", async (int id, YuzzContext db) =>
        {
            var battery = await db.Batteries
                .Include(b => b.Bess)
                .ThenInclude(b => b.OperationMode)
                .FirstOrDefaultAsync(b => b.Id == id);

            return battery is not null ? Results.Ok(battery) : Results.NotFound();
        });

        // GET: by BESS ID
        batteryGroup.MapGet("/bess/{bessId:int}", async (int bessId, YuzzContext db) =>
        {
            var batteries = await db.Batteries
                .Include(b => b.Bess)
                .ThenInclude(b => b.OperationMode)
                .Where(b => b.BessId == bessId)
                .ToListAsync();
            return Results.Ok(batteries);
        });

        // PUT: Update a battery
        batteryGroup.MapPut("/{id:int}", async (int id, Battery updatedBattery, YuzzContext db) =>
        {
            var battery = await db.Batteries.FindAsync(id);
            if (battery is null) return Results.NotFound();

            // Validate that the BESS exists
            if (updatedBattery.BessId != battery.BessId)
            {
                var bessExists = await db.Besses.AnyAsync(b => b.Id == updatedBattery.BessId);
                if (!bessExists) return Results.BadRequest("The specified BESS does not exist");
            }

            // Validate unique ModbusId (if changed)
            if (updatedBattery.ModbusId != battery.ModbusId)
            {
                var modbusExists = await db.Batteries.AnyAsync(b => b.ModbusId == updatedBattery.ModbusId && b.Id != id);
                if (modbusExists) return Results.BadRequest("The ModbusId is already in use");
            }

            // Update properties based on the Battery model
            battery.ModbusId = updatedBattery.ModbusId;
            battery.Name = updatedBattery.Name;
            battery.SocMax = updatedBattery.SocMax;
            battery.SocMin = updatedBattery.SocMin;
            battery.CurrentMax = updatedBattery.CurrentMax;
            battery.VoltageMax = updatedBattery.VoltageMax;
            battery.VoltageMin = updatedBattery.VoltageMin;
            battery.VoltageAbsorption = updatedBattery.VoltageAbsorption;
            battery.CurrentCharging = updatedBattery.CurrentCharging;
            battery.PwrMax = updatedBattery.PwrMax;
            battery.BessId = updatedBattery.BessId;

            await db.SaveChangesAsync();
            return Results.Ok(battery);
        });
    }

    private static void MapPcsEndpoints(WebApplication app)
    {
        var pcsGroup = app.MapGroup("/api/pcs").RequireAuthorization("ApiAuth");

        // GET: all PCS
        pcsGroup.MapGet("/", async (YuzzContext db) =>
        {
            var pcs = await db.Pcs
                .Include(p => p.Battery)
                .Include(p => p.PcsModel)
                .ToListAsync();
            return Results.Ok(pcs);
        });

        // GET: by id
        pcsGroup.MapGet("/{id:int}", async (int id, YuzzContext db) =>
        {
            var pcs = await db.Pcs
                .Include(p => p.Battery)
                .Include(p => p.PcsModel)
                .FirstOrDefaultAsync(p => p.Id == id);

            return pcs is not null ? Results.Ok(pcs) : Results.NotFound();
        });

        // GET: by Battery ID
        pcsGroup.MapGet("/battery/{batteryId:int}", async (int batteryId, YuzzContext db) =>
        {
            var pcs = await db.Pcs
                .Include(p => p.Battery)
                .ThenInclude(b => b.Bess)
                .Include(p => p.PcsModel)
                .Where(p => p.BatteryId == batteryId)
                .ToListAsync();
            return Results.Ok(pcs);
        });

        // PUT: Update a PCS
        pcsGroup.MapPut("/{id:int}", async (int id, Pcs updatedPcs, YuzzContext db) =>
        {
            var pcs = await db.Pcs.FindAsync(id);
            if (pcs is null) return Results.NotFound();

            // Validate that the Battery exists
            if (updatedPcs.BatteryId != pcs.BatteryId)
            {
                var batteryExists = await db.Batteries.AnyAsync(b => b.Id == updatedPcs.BatteryId);
                if (!batteryExists) return Results.BadRequest("The specified battery does not exist");
            }

            // Validate that the PcsModel exists
            if (updatedPcs.PcsModelId != pcs.PcsModelId)
            {
                var modelExists = await db.PcsModels.AnyAsync(m => m.Id == updatedPcs.PcsModelId);
                if (!modelExists) return Results.BadRequest("The specified PCS model does not exist");
            }

            // Validate unique CanId (if changed)
            if (updatedPcs.CanId != pcs.CanId)
            {
                var canExists = await db.Pcs.AnyAsync(p => p.CanId == updatedPcs.CanId && p.Id != id);
                if (canExists) return Results.BadRequest("The CanId is already in use");
            }

            // Update properties based on the Pcs model
            pcs.CanId = updatedPcs.CanId;
            pcs.BatteryId = updatedPcs.BatteryId;
            pcs.PcsModelId = updatedPcs.PcsModelId;

            await db.SaveChangesAsync();
            return Results.Ok(pcs);
        });
    }

    private static void MapBessEndpoints(WebApplication app)
    {
        var bessGroup = app.MapGroup("/api/bess").RequireAuthorization("ApiAuth");

        // GET: all BESS
        bessGroup.MapGet("/", async (YuzzContext db) =>
        {
            var besses = await db.Besses
                .Include(b => b.OperationMode)
                .ToListAsync();
            return Results.Ok(besses);
        });

        // GET: by ID
        bessGroup.MapGet("/{id:int}", async (int id, YuzzContext db) =>
        {
            var bess = await db.Besses
                .Include(b => b.OperationMode)
                .FirstOrDefaultAsync(b => b.Id == id);

            return bess is not null ? Results.Ok(bess) : Results.NotFound();
        });
    }
}