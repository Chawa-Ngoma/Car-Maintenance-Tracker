using System;
using System.Collections.Generic;

namespace CarMaintenanceTracker.Models;

public class Vehicle : ValidatableModel
{
    private const int MinimumYear = 1900;

    private string _make = string.Empty;
    private string _model = string.Empty;
    private int _year;
    private string _nickname = string.Empty;
    private string _plate = string.Empty;

    public int Id { get; set; }

    public string Make
    {
        get => _make;
        set => SetField(ref _make, value, string.IsNullOrWhiteSpace(value) ? "Make is required." : null);
    }

    public string Model
    {
        get => _model;
        set => SetField(ref _model, value, string.IsNullOrWhiteSpace(value) ? "Model is required." : null);
    }

    public int Year
    {
        get => _year;
        set => SetField(ref _year, value,
            value < MinimumYear || value > DateTime.Now.Year + 1
                ? $"Year must be between {MinimumYear} and {DateTime.Now.Year + 1}."
                : null);
    }

    public string Nickname
    {
        get => _nickname;
        set => SetField(ref _nickname, value, null);
    }

    public string Plate
    {
        get => _plate;
        set => SetField(ref _plate, value, null);
    }

    public List<ServiceEntry> ServiceEntries { get; set; } = new();
}
