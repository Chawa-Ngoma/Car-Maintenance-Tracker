using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarMaintenanceTracker.Data;
using CarMaintenanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CarMaintenanceTracker.Services;

/// <summary>
/// Only class allowed to talk to AppDbContext. Views and ViewModels go through
/// this repository instead.
/// </summary>
public class ServiceLogRepository : IServiceLogRepository
{
    private readonly DbContextOptions<AppDbContext> _options;

    public ServiceLogRepository(DbContextOptions<AppDbContext> options)
    {
        _options = options;
    }

    public async Task<List<Vehicle>> GetVehiclesAsync()
    {
        await using var context = new AppDbContext(_options);
        return await context.Vehicles.AsNoTracking().ToListAsync();
    }

    public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
    {
        await using var context = new AppDbContext(_options);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();
        return vehicle;
    }

    public async Task UpdateVehicleAsync(Vehicle vehicle)
    {
        await using var context = new AppDbContext(_options);
        context.Vehicles.Update(vehicle);
        await context.SaveChangesAsync();
    }

    public async Task DeleteVehicleAsync(int vehicleId)
    {
        await using var context = new AppDbContext(_options);
        var vehicle = await context.Vehicles.FindAsync(vehicleId);
        if (vehicle is null)
        {
            return;
        }

        context.Vehicles.Remove(vehicle);
        await context.SaveChangesAsync();
    }

    public async Task<List<ServiceEntry>> GetServiceEntriesAsync(int vehicleId)
    {
        await using var context = new AppDbContext(_options);
        return await context.ServiceEntries
            .Where(e => e.VehicleId == vehicleId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ServiceEntry> AddServiceEntryAsync(ServiceEntry entry)
    {
        await using var context = new AppDbContext(_options);
        context.ServiceEntries.Add(entry);
        await context.SaveChangesAsync();
        return entry;
    }

    public async Task UpdateServiceEntryAsync(ServiceEntry entry)
    {
        await using var context = new AppDbContext(_options);
        context.ServiceEntries.Update(entry);
        await context.SaveChangesAsync();
    }

    public async Task DeleteServiceEntryAsync(int serviceEntryId)
    {
        await using var context = new AppDbContext(_options);
        var entry = await context.ServiceEntries.FindAsync(serviceEntryId);
        if (entry is null)
        {
            return;
        }

        context.ServiceEntries.Remove(entry);
        await context.SaveChangesAsync();
    }
}
