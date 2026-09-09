using System.Windows;
using System.Windows.Controls;
using CarMaintenanceTracker.ViewModels;

namespace CarMaintenanceTracker.Views;

/// <summary>
/// Interaction logic for ServiceEntryEditView.xaml. DataContext is inherited from
/// the parent (ServiceLogViewModel) — the delete confirmation is the only
/// code-behind logic, unavoidable since MessageBox is a UI concern.
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

        var result = MessageBox.Show(
            $"Delete this '{viewModel.SelectedEntry.ServiceType}' entry?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            viewModel.DeleteEntryCommand.Execute(null);
        }
    }
}
