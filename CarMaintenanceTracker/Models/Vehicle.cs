using System.Collections.Generic;

namespace CarMaintenanceTracker.Models;

public class Vehicle
{
    public int Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;

    public List<ServiceEntry> ServiceEntries { get; set; } = new();
}
