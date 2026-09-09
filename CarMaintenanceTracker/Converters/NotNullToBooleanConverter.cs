using System;
using System.Globalization;
using System.Windows.Data;

namespace CarMaintenanceTracker.Converters;

/// <summary>True when the bound value is non-null; used to enable/disable an edit panel based on selection.</summary>
public class NotNullToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is not null;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
