using System;

namespace CarMaintenanceTracker.Models;

public class ServiceEntry : ValidatableModel
{
    private DateTime _date;
    private int _odometer;
    private string _serviceType = string.Empty;
    private decimal _cost;
    private string _notes = string.Empty;

    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public DateTime Date
    {
        get => _date;
        set => SetField(ref _date, value, value == default ? "Date is required." : null);
    }

    public int Odometer
    {
        get => _odometer;
        set => SetField(ref _odometer, value, value < 0 ? "Odometer cannot be negative." : null);
    }

    public string ServiceType
    {
        get => _serviceType;
        set => SetField(ref _serviceType, value, string.IsNullOrWhiteSpace(value) ? "Service type is required." : null);
    }

    public decimal Cost
    {
        get => _cost;
        set => SetField(ref _cost, value, value < 0 ? "Cost cannot be negative." : null);
    }

    public string Notes
    {
        get => _notes;
        set => SetField(ref _notes, value, null);
    }
}
