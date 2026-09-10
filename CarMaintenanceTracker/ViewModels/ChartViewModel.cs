using System.ComponentModel;
using System.Globalization;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CarMaintenanceTracker.ViewModels;

/// <summary>
/// Shapes ServiceLogViewModel's entries into an OxyPlot cost-over-time chart for
/// the currently selected vehicle. Takes ServiceLogViewModel itself rather than
/// the repository, and rebuilds whenever ServiceLogViewModel's existing summary
/// properties change (Phase 3's SelectedVehicleChanged -> SetSelectedVehicleAsync
/// wiring already drives those on vehicle switch, add, edit, and delete).
/// </summary>
public class ChartViewModel : ObservableObject
{
    private const int MinimumPointsForTrend = 2;

    // Must match Styles/Colors.xaml's AccentBrush (#C8372E) per DESIGN-SPEC.md.
    // OxyPlot series colors are plain OxyColor values set in code, not WPF
    // resources, so this is a deliberate, documented duplication rather than a
    // shared token -- ViewModels don't reach into Application.Resources.
    private static readonly OxyColor AccentColor = OxyColor.Parse("#C8372E");
    private static readonly OxyColor TextColor = OxyColor.Parse("#22262B");
    private static readonly OxyColor GridlineColor = OxyColor.Parse("#E0DED8");

    private readonly ServiceLogViewModel _serviceLogViewModel;
    private PlotModel _plotModel;

    public PlotModel PlotModel
    {
        get => _plotModel;
        private set => SetField(ref _plotModel, value);
    }

    public bool HasEnoughData => _serviceLogViewModel.EntryCount >= MinimumPointsForTrend;

    public ChartViewModel(ServiceLogViewModel serviceLogViewModel)
    {
        _serviceLogViewModel = serviceLogViewModel;
        _serviceLogViewModel.PropertyChanged += OnServiceLogViewModelPropertyChanged;
        _plotModel = BuildPlotModel();
    }

    private void OnServiceLogViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ServiceLogViewModel.EntryCount)
            or nameof(ServiceLogViewModel.TotalCost)
            or nameof(ServiceLogViewModel.LastServiceDate))
        {
            PlotModel = BuildPlotModel();
            OnPropertyChanged(nameof(HasEnoughData));
        }
    }

    private PlotModel BuildPlotModel()
    {
        var model = new PlotModel
        {
            PlotAreaBorderThickness = new OxyThickness(0),
            Background = OxyColors.Transparent,
            PlotAreaBackground = OxyColors.Transparent,
            TextColor = TextColor
        };

        model.Axes.Add(new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            StringFormat = "MMM d",
            TextColor = TextColor,
            AxislineColor = GridlineColor,
            TicklineColor = GridlineColor,
            MajorGridlineColor = GridlineColor
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            // WPF's binding StringFormat=C (used by the Total Spent card) resolves
            // against en-US regardless of OS locale, but OxyPlot's own StringFormat
            // uses CurrentCulture -- explicit LabelFormatter keeps the two consistent
            // instead of the axis showing "R" (Rand) while the summary card shows "$".
            LabelFormatter = value => value.ToString("C0", CultureInfo.GetCultureInfo("en-US")),
            TextColor = TextColor,
            AxislineColor = GridlineColor,
            TicklineColor = GridlineColor,
            MajorGridlineColor = GridlineColor,
            MinimumPadding = 0.1,
            MaximumPadding = 0.1
        });

        if (!HasEnoughData)
        {
            return model;
        }

        var series = new LineSeries
        {
            Color = AccentColor,
            MarkerType = MarkerType.Circle,
            MarkerFill = AccentColor,
            MarkerSize = 4,
            StrokeThickness = 2
        };

        foreach (var entry in _serviceLogViewModel.ServiceEntries.OrderBy(entry => entry.Date))
        {
            series.Points.Add(DateTimeAxis.CreateDataPoint(entry.Date, (double)entry.Cost));
        }

        model.Series.Add(series);
        return model;
    }
}
