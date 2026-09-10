using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using CarMaintenanceTracker.ViewModels;

namespace CarMaintenanceTracker.Views;

/// <summary>
/// Renders toasts raised via ToastService. Creating/removing the toast Border
/// and running its auto-dismiss timer is unavoidable WPF UI code-behind --
/// there is no ViewModel-level concept of "a transient overlay element" to bind to.
/// </summary>
public partial class ToastHost : UserControl
{
    private static readonly TimeSpan DisplayDuration = TimeSpan.FromSeconds(3);

    public ToastHost()
    {
        InitializeComponent();
        ToastService.Toast += OnToast;
        Unloaded += (_, _) => ToastService.Toast -= OnToast;
    }

    private void OnToast(object? sender, ToastMessage message) =>
        Dispatcher.Invoke(() => ShowToast(message));

    private void ShowToast(ToastMessage message)
    {
        var brushKey = message.Type == ToastType.Success ? "SuccessBrush" : "DangerBrush";

        var border = new Border
        {
            Background = (Brush)FindResource(brushKey),
            CornerRadius = new CornerRadius(4),
            Padding = new Thickness(14, 10, 14, 10),
            Margin = new Thickness(0, 8, 0, 0),
            Child = new TextBlock
            {
                Text = message.Text,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 300,
                FontFamily = (FontFamily)FindResource("AppFontFamily")
            }
        };

        ToastPanel.Children.Add(border);

        var timer = new DispatcherTimer { Interval = DisplayDuration };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            ToastPanel.Children.Remove(border);
        };
        timer.Start();
    }
}
