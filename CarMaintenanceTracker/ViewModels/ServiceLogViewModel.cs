using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CarMaintenanceTracker.Models;
using CarMaintenanceTracker.Services;

namespace CarMaintenanceTracker.ViewModels;

/// <summary>
/// Shows the service log for whichever vehicle is currently selected elsewhere.
/// Has no reference to <see cref="MainViewModel"/> — the composition root wires
/// <see cref="SetSelectedVehicleAsync"/> to MainViewModel.SelectedVehicleChanged,
/// so this class only depends on a Vehicle, not on who selected it.
/// </summary>
public class ServiceLogViewModel : ObservableObject
{
    private readonly IServiceLogRepository _repository;
    private Vehicle? _selectedVehicle;
    private ServiceEntry? _selectedEntry;

    public ObservableCollection<ServiceEntry> ServiceEntries { get; } = new();

    public ServiceEntry? SelectedEntry
    {
        get => _selectedEntry;
        set => SetField(ref _selectedEntry, value);
    }

    public ICommand AddEntryCommand { get; }
    public ICommand SaveEntryCommand { get; }
    public ICommand DeleteEntryCommand { get; }

    public ServiceLogViewModel(IServiceLogRepository repository)
    {
        _repository = repository;

        AddEntryCommand = new RelayCommand(async () => await AddEntryAsync(), () => _selectedVehicle is not null);
        SaveEntryCommand = new RelayCommand(async () => await SaveSelectedEntryAsync(), () => SelectedEntry is not null);
        DeleteEntryCommand = new RelayCommand(async () => await DeleteSelectedEntryAsync(), () => SelectedEntry is not null);
    }

    public async Task SetSelectedVehicleAsync(Vehicle? vehicle)
    {
        _selectedVehicle = vehicle;
        SelectedEntry = null;
        ServiceEntries.Clear();

        if (vehicle is null)
        {
            return;
        }

        foreach (var entry in await _repository.GetServiceEntriesAsync(vehicle.Id))
        {
            ServiceEntries.Add(entry);
        }
    }

    private async Task AddEntryAsync()
    {
        if (_selectedVehicle is null)
        {
            return;
        }

        var entry = new ServiceEntry
        {
            VehicleId = _selectedVehicle.Id,
            Date = DateTime.Today,
            Odometer = 0,
            ServiceType = "New Service",
            Cost = 0m,
            Notes = string.Empty
        };

        await _repository.AddServiceEntryAsync(entry);
        ServiceEntries.Add(entry);
        SelectedEntry = entry;
    }

    private async Task SaveSelectedEntryAsync()
    {
        if (SelectedEntry is null)
        {
            return;
        }

        await _repository.UpdateServiceEntryAsync(SelectedEntry);
    }

    private async Task DeleteSelectedEntryAsync()
    {
        if (SelectedEntry is null)
        {
            return;
        }

        await _repository.DeleteServiceEntryAsync(SelectedEntry.Id);
        ServiceEntries.Remove(SelectedEntry);
        SelectedEntry = null;
    }
}
