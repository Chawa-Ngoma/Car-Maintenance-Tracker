using System.Windows;

namespace CarMaintenanceTracker.Views;

/// <summary>
/// Themed replacement for MessageBox.Show confirmations -- one reusable dialog
/// for every "are you sure" prompt in the app (delete a vehicle, delete an
/// entry) rather than a one-off per call site.
/// </summary>
public partial class ConfirmationDialog : Window
{
    public bool Confirmed { get; private set; }

    private ConfirmationDialog(string message, string confirmLabel, string cancelLabel, bool isDestructive)
    {
        InitializeComponent();
        MessageTextBlock.Text = message;
        ConfirmButton.Content = confirmLabel;
        CancelButton.Content = cancelLabel;
        ConfirmButton.Style = (Style)FindResource(isDestructive ? "DeleteButtonStyle" : "PrimaryButtonStyle");
    }

    /// <summary>Shows the dialog modally and returns true if the user confirmed.</summary>
    public static bool Show(Window owner, string message, string confirmLabel = "Confirm", string cancelLabel = "Cancel", bool isDestructive = true)
    {
        var dialog = new ConfirmationDialog(message, confirmLabel, cancelLabel, isDestructive) { Owner = owner };
        dialog.ShowDialog();
        return dialog.Confirmed;
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        Confirmed = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Confirmed = false;
        Close();
    }
}
