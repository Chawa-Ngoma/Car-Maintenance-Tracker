using System;
using System.Globalization;
using System.Windows.Data;

namespace CarMaintenanceTracker.Converters;

/// <summary>
/// Maps a service type string to an icon glyph per DESIGN-SPEC.md's service-type
/// icon mapping table. Uses standard Unicode emoji (rendered via the system's
/// Segoe UI Emoji font) rather than an icon font/library, since the spec's shapes
/// (droplet, wrench, clipboard, wheel, battery, car) map directly onto existing
/// Unicode characters without needing a private-use-area icon font's codepoints.
/// </summary>
public class ServiceTypeToIconConverter : IValueConverter
{
    private const string OilChangeIcon = "\U0001F4A7"; // droplet
    private const string BrakeIcon = "\U0001F527"; // wrench
    private const string InspectionIcon = "\U0001F4CB"; // clipboard
    private const string TireIcon = "\U0001F6DE"; // wheel
    private const string BatteryIcon = "\U0001F50B"; // battery
    private const string DefaultIcon = "\U0001F697"; // car

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var serviceType = value as string ?? string.Empty;

        return serviceType switch
        {
            _ when Contains(serviceType, "oil") => OilChangeIcon,
            _ when Contains(serviceType, "brake") => BrakeIcon,
            _ when Contains(serviceType, "inspect") || Contains(serviceType, "service") => InspectionIcon,
            _ when Contains(serviceType, "tyre") || Contains(serviceType, "tire") => TireIcon,
            _ when Contains(serviceType, "battery") => BatteryIcon,
            _ => DefaultIcon
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();

    private static bool Contains(string source, string term) =>
        source.Contains(term, StringComparison.OrdinalIgnoreCase);
}
