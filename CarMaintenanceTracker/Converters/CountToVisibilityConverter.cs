using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CarMaintenanceTracker.Converters;

/// <summary>Visible when the bound count is zero -- used to show an empty-state placeholder in place of an empty list.</summary>
public class CountToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is int count && count == 0 ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
