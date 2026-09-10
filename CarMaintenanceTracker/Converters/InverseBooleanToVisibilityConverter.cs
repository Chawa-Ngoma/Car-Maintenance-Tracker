using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CarMaintenanceTracker.Converters;

/// <summary>True becomes Collapsed, false becomes Visible -- used to show a placeholder when a bound "has data" flag is false.</summary>
public class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
