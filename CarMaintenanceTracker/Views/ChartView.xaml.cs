using System.Windows.Controls;

namespace CarMaintenanceTracker.Views;

/// <summary>
/// Interaction logic for ChartView.xaml. DataContext (a ChartViewModel) is
/// inherited from the parent panel, set in MainWindow's code-behind.
/// </summary>
public partial class ChartView : UserControl
{
    public ChartView()
    {
        InitializeComponent();
    }
}
