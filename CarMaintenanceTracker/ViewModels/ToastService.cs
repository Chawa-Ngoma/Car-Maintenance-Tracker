using System;

namespace CarMaintenanceTracker.ViewModels;

public record ToastMessage(string Text, ToastType Type);

/// <summary>
/// Lightweight publish/subscribe notification bus so ViewModels can raise
/// save-success/error feedback without holding a reference to any WPF UI
/// element. Views/ToastHost subscribes and renders the actual toast.
/// </summary>
public static class ToastService
{
    public static event EventHandler<ToastMessage>? Toast;

    public static void ShowSuccess(string message) => Toast?.Invoke(null, new ToastMessage(message, ToastType.Success));

    public static void ShowError(string message) => Toast?.Invoke(null, new ToastMessage(message, ToastType.Error));
}
