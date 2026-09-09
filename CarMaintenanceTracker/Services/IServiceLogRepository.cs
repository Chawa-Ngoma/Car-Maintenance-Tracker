using System.Collections.Generic;
using System.Threading.Tasks;
using CarMaintenanceTracker.Models;

namespace CarMaintenanceTracker.Services;

public interface IServiceLogRepository
{
    Task<List<Vehicle>> GetVehiclesAsync();
    Task<Vehicle?> GetVehicleAsync(int vehicleId);
    Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
    Task UpdateVehicleAsync(Vehicle vehicle);
    Task DeleteVehicleAsync(int vehicleId);

    Task<List<ServiceEntry>> GetServiceEntriesAsync(int vehicleId);
    Task<ServiceEntry> AddServiceEntryAsync(ServiceEntry entry);
    Task UpdateServiceEntryAsync(ServiceEntry entry);
    Task DeleteServiceEntryAsync(int serviceEntryId);
}
