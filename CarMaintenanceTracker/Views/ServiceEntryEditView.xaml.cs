using System.Windows;
using System.Windows.Controls;
using CarMaintenanceTracker.ViewModels;

namespace CarMaintenanceTracker.Views;

/// <summary>
/// Interaction logic for ServiceEntryEditView.xaml. DataContext is inherited from
/// the parent (ServiceLogViewModel) — the delete confirmation is the only
/// code-behind logic, unavoidable since showing a dialog is a UI concern.
/// </summary>
public partial class ServiceEntryEditView : UserControl
{
    public ServiceEntryEditView()
    {
        InitializeComponent();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ServiceLogViewModel viewModel || viewModel.SelectedEntry is null)
        {
            return;
        }

        var owner = Window.GetWindow(this);
        var confirmed = ConfirmationDialog.Show(
            owner,
            $"Delete this '{viewModel.SelectedEntry.ServiceType}' entry?",
            confirmLabel: "Delete");

        if (confirmed)
        {
            viewModel.DeleteEntryCommand.Execute(null);
        }
    }
}
