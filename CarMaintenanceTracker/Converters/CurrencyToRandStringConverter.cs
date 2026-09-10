using System;
using System.Globalization;
using System.Windows.Data;

namespace CarMaintenanceTracker.Converters;

/// <summary>
/// Formats a decimal cost as South African Rand explicitly (e.g. "R 410,00"),
/// independent of the OS/thread's current culture or the binding's ambient
/// Language -- WPF's plain StringFormat=C previously resolved to en-US ($) on
/// this machine despite the app being Rand-only, which is exactly the bug this
/// converter avoids by never depending on an ambient culture.
/// </summary>
public class CurrencyToRandStringConverter : IValueConverter
{
    private static readonly CultureInfo RandCulture = CultureInfo.GetCultureInfo("en-ZA");

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var amount = value switch
        {
            decimal d => d,
            int i => i,
            _ => 0m
        };

        return amount.ToString("C", RandCulture);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
