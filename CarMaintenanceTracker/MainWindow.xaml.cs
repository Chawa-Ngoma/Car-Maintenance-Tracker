using System.Windows;
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
}
