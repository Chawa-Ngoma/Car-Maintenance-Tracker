using System;

namespace CarMaintenanceTracker.Models;

public class ServiceEntry
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public DateTime Date { get; set; }
    public int Odometer { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Notes { get; set; } = string.Empty;
}
