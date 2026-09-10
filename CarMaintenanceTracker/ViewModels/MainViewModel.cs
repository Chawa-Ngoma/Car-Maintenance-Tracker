using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CarMaintenanceTracker.Models;
using CarMaintenanceTracker.Services;

namespace CarMaintenanceTracker.ViewModels;

/// <summary>
/// Owns the vehicle list and which vehicle is selected. Raises
/// <see cref="SelectedVehicleChanged"/> so other view models (e.g. the service log)
/// can react without holding a reference to this class.
/// </summary>
public class MainViewModel : ObservableObject
{
    private const int DefaultNewVehicleYear = 2000;

    private readonly IServiceLogRepository _repository;
    private Vehicle? _selectedVehicle;

    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    public Vehicle? SelectedVehicle
    {
        get => _selectedVehicle;
        set
        {
            if (SetField(ref _selectedVehicle, value))
            {
                SelectedVehicleChanged?.Invoke(this, value);
            }
        }
    }

    public event EventHandler<Vehicle?>? SelectedVehicleChanged;

    public ICommand AddVehicleCommand { get; }
    public ICommand SaveVehicleCommand { get; }
    public ICommand DeleteVehicleCommand { get; }

    public MainViewModel(IServiceLogRepository repository)
    {
        _repository = repository;

        AddVehicleCommand = new RelayCommand(async () => await AddVehicleAsync());
        SaveVehicleCommand = new RelayCommand(async () => await SaveSelectedVehicleAsync(), () => SelectedVehicle is not null);
        DeleteVehicleCommand = new RelayCommand(async () => await DeleteSelectedVehicleAsync(), () => SelectedVehicle is not null);
    }

    public async Task LoadVehiclesAsync()
    {
        Vehicles.Clear();
        foreach (var vehicle in await _repository.GetVehiclesAsync())
        {
            Vehicles.Add(vehicle);
        }
    }

    private async Task AddVehicleAsync()
    {
        var vehicle = new Vehicle
        {
            Make = "New Make",
            Model = "New Model",
            Year = DefaultNewVehicleYear,
            Nickname = "New Vehicle",
            Plate = string.Empty
        };

        await _repository.AddVehicleAsync(vehicle);
        Vehicles.Add(vehicle);
        SelectedVehicle = vehicle;
    }

    private async Task SaveSelectedVehicleAsync()
    {
        if (SelectedVehicle is null)
        {
            return;
        }

        if (SelectedVehicle.HasErrors)
        {
            ToastService.ShowValidationError();
            return;
        }

        await _repository.UpdateVehicleAsync(SelectedVehicle);
        ToastService.ShowSuccess("Vehicle saved");
    }

    private async Task DeleteSelectedVehicleAsync()
    {
        if (SelectedVehicle is null)
        {
            return;
        }

        await _repository.DeleteVehicleAsync(SelectedVehicle.Id);
        Vehicles.Remove(SelectedVehicle);
        SelectedVehicle = null;
    }
}
