using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
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

    /// <summary>Sorted view over ServiceEntries -- the View binds to this instead of the raw collection.</summary>
    public ICollectionView ServiceEntriesView { get; }

    public ServiceEntry? SelectedEntry
    {
        get => _selectedEntry;
        set => SetField(ref _selectedEntry, value);
    }

    public ICommand AddEntryCommand { get; }
    public ICommand SaveEntryCommand { get; }
    public ICommand DeleteEntryCommand { get; }
    public ICommand SortByDateCommand { get; }
    public ICommand SortByOdometerCommand { get; }

    /// <summary>Sum of all entries' Cost, for the "Total spent" summary card.</summary>
    public decimal TotalCost => ServiceEntries.Sum(entry => entry.Cost);

    /// <summary>Number of entries, for the "Entry count" summary card.</summary>
    public int EntryCount => ServiceEntries.Count;

    /// <summary>Most recent entry date, for the "Last service date" summary card.</summary>
    public DateTime? LastServiceDate => ServiceEntries.Count == 0 ? null : ServiceEntries.Max(entry => entry.Date);

    /// <summary>Message shown in place of the list when there are no entries to display.</summary>
    public string EmptyStateMessage => _selectedVehicle is null
        ? "Select a vehicle to see its service history."
        : "No service entries yet — click Add New below to log one.";

    public ServiceLogViewModel(IServiceLogRepository repository)
    {
        _repository = repository;

        ServiceEntriesView = CollectionViewSource.GetDefaultView(ServiceEntries);
        ServiceEntriesView.SortDescriptions.Add(new SortDescription(nameof(ServiceEntry.Date), ListSortDirection.Descending));

        AddEntryCommand = new RelayCommand(async () => await AddEntryAsync(), () => _selectedVehicle is not null);
        SaveEntryCommand = new RelayCommand(async () => await SaveSelectedEntryAsync(), () => SelectedEntry is not null);
        DeleteEntryCommand = new RelayCommand(async () => await DeleteSelectedEntryAsync(), () => SelectedEntry is not null);
        SortByDateCommand = new RelayCommand(() => SetSort(nameof(ServiceEntry.Date), ListSortDirection.Descending));
        SortByOdometerCommand = new RelayCommand(() => SetSort(nameof(ServiceEntry.Odometer), ListSortDirection.Descending));

        ServiceEntries.CollectionChanged += (_, _) => RaiseSummaryPropertiesChanged();
    }

    private void SetSort(string propertyName, ListSortDirection direction)
    {
        ServiceEntriesView.SortDescriptions.Clear();
        ServiceEntriesView.SortDescriptions.Add(new SortDescription(propertyName, direction));
    }

    private void RaiseSummaryPropertiesChanged()
    {
        OnPropertyChanged(nameof(TotalCost));
        OnPropertyChanged(nameof(EntryCount));
        OnPropertyChanged(nameof(LastServiceDate));
        OnPropertyChanged(nameof(EmptyStateMessage));
    }

    public async Task SetSelectedVehicleAsync(Vehicle? vehicle)
    {
        _selectedVehicle = vehicle;
        SelectedEntry = null;
        ServiceEntries.Clear();
        OnPropertyChanged(nameof(EmptyStateMessage));

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

        if (SelectedEntry.HasErrors)
        {
            ToastService.ShowError("Couldn't save — check the highlighted fields");
            return;
        }

        await _repository.UpdateServiceEntryAsync(SelectedEntry);
        RaiseSummaryPropertiesChanged(); // editing Cost/Date in place doesn't raise CollectionChanged
        ToastService.ShowSuccess("Service entry saved");
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
