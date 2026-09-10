using System.Windows;
using System.Windows.Controls;
using CarMaintenanceTracker.ViewModels;

namespace CarMaintenanceTracker.Views;

/// <summary>
/// Interaction logic for VehicleEditView.xaml. DataContext is inherited from the
/// parent (MainViewModel) — the only code-behind logic here is the delete
/// confirmation, which has to live in code-behind because showing a dialog is a UI concern.
/// </summary>
public partial class VehicleEditView : UserControl
{
    public VehicleEditView()
    {
        InitializeComponent();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel viewModel || viewModel.SelectedVehicle is null)
        {
            return;
        }

        var owner = Window.GetWindow(this);
        var confirmed = ConfirmationDialog.Show(
            owner,
            $"Delete '{viewModel.SelectedVehicle.Nickname}' and all of its service entries?",
            confirmLabel: "Delete");

        if (confirmed)
        {
            viewModel.DeleteVehicleCommand.Execute(null);
        }
    }
}
