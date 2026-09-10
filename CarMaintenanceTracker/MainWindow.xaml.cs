using System.Windows;
using CarMaintenanceTracker.Models;
using CarMaintenanceTracker.ViewModels;

namespace CarMaintenanceTracker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel, ServiceLogViewModel serviceLogViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
        ServiceLogPanel.DataContext = serviceLogViewModel;
    }

    /// <summary>
    /// Per-row edit icon-button: selects the entry, which is what the edit panel
    /// below already reacts to. No new behavior, just a visual shortcut to the
    /// same ServiceLogViewModel.SelectedEntry the row-click selection already sets.
    /// </summary>
    private void EditEntryButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: ServiceEntry entry } && ServiceLogPanel.DataContext is ServiceLogViewModel viewModel)
        {
            viewModel.SelectedEntry = entry;
        }
    }
}
