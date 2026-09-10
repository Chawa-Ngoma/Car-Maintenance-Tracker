using System;
using System.IO;
using System.Windows;
using CarMaintenanceTracker.Data;
using CarMaintenanceTracker.Services;
using CarMaintenanceTracker.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarMaintenanceTracker;

/// <summary>
/// Composition root: builds the EF Core options, the repository, and the view
/// models, and wires MainViewModel's selection to the service log view model.
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var dbPath = Path.Combine(AppContext.BaseDirectory, AppDbContext.DatabaseFileName);
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
        var options = optionsBuilder.Options;

        using (var context = new AppDbContext(options))
        {
            context.Database.Migrate();
        }

        IServiceLogRepository repository = new ServiceLogRepository(options);
        var mainViewModel = new MainViewModel(repository);
        var serviceLogViewModel = new ServiceLogViewModel(repository);
        var chartViewModel = new ChartViewModel(serviceLogViewModel);

        mainViewModel.SelectedVehicleChanged += async (_, vehicle) =>
            await serviceLogViewModel.SetSelectedVehicleAsync(vehicle);

        var mainWindow = new MainWindow(mainViewModel, serviceLogViewModel, chartViewModel);
        mainWindow.Show();

        _ = mainViewModel.LoadVehiclesAsync();
    }
}
