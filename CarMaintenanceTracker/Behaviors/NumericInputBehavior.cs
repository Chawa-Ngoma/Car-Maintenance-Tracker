using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarMaintenanceTracker.Behaviors;

public enum NumericInputMode
{
    Integer,
    Decimal
}

/// <summary>
/// Attached behavior that blocks keystrokes/paste that would make a TextBox's
/// value invalid, instead of only catching it on save. One parameterized
/// implementation shared by Odometer (Integer), Cost (Decimal), and Year
/// (Integer, MaxLength=4) rather than a PreviewTextInput handler per field.
/// </summary>
public static class NumericInputBehavior
{
    public static readonly DependencyProperty ModeProperty = DependencyProperty.RegisterAttached(
        "Mode", typeof(NumericInputMode?), typeof(NumericInputBehavior),
        new PropertyMetadata(null, OnModeChanged));

    public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached(
        "MaxLength", typeof(int), typeof(NumericInputBehavior), new PropertyMetadata(0));

    public static readonly DependencyProperty MaxDecimalPlacesProperty = DependencyProperty.RegisterAttached(
        "MaxDecimalPlaces", typeof(int), typeof(NumericInputBehavior), new PropertyMetadata(2));

    public static NumericInputMode? GetMode(DependencyObject element) => (NumericInputMode?)element.GetValue(ModeProperty);
    public static void SetMode(DependencyObject element, NumericInputMode? value) => element.SetValue(ModeProperty, value);

    public static int GetMaxLength(DependencyObject element) => (int)element.GetValue(MaxLengthProperty);
    public static void SetMaxLength(DependencyObject element, int value) => element.SetValue(MaxLengthProperty, value);

    public static int GetMaxDecimalPlaces(DependencyObject element) => (int)element.GetValue(MaxDecimalPlacesProperty);
    public static void SetMaxDecimalPlaces(DependencyObject element, int value) => element.SetValue(MaxDecimalPlacesProperty, value);

    private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBox textBox)
        {
            return;
        }

        textBox.PreviewTextInput -= OnPreviewTextInput;
        DataObject.RemovePastingHandler(textBox, OnPaste);

        if (e.NewValue is NumericInputMode)
        {
            textBox.PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(textBox, OnPaste);
        }
    }

    private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var textBox = (TextBox)sender;
        var insertedText = NormalizeDecimalSeparator(textBox, e.Text);

        if (!IsValid(textBox, GetProposedText(textBox, insertedText)))
        {
            e.Handled = true;
            return;
        }

        // Only take over the insertion ourselves when we actually rewrote the
        // character (comma -> period); otherwise let WPF insert e.Text normally.
        if (insertedText != e.Text)
        {
            InsertText(textBox, insertedText);
            e.Handled = true;
        }
    }

    private static void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        var textBox = (TextBox)sender;

        if (!e.DataObject.GetDataPresent(DataFormats.Text) ||
            e.DataObject.GetData(DataFormats.Text) is not string pastedText)
        {
            e.CancelCommand();
            return;
        }

        var normalizedText = NormalizeDecimalSeparator(textBox, pastedText);

        if (!IsValid(textBox, GetProposedText(textBox, normalizedText)))
        {
            e.CancelCommand();
            return;
        }

        if (normalizedText != pastedText)
        {
            InsertText(textBox, normalizedText);
            e.CancelCommand(); // we already performed the (normalized) insertion ourselves
        }
    }

    /// <summary>
    /// South African number formatting conventionally uses "," as the decimal
    /// separator (and Windows' numpad decimal key emits "," rather than "." on
    /// this locale), so Decimal mode accepts either character but always stores
    /// "." -- keeping the field's value locale-independent, consistent with
    /// Converters/CurrencyToRandStringConverter's explicit-formatting approach.
    /// </summary>
    private static string NormalizeDecimalSeparator(DependencyObject textBox, string text) =>
        GetMode(textBox) == NumericInputMode.Decimal ? text.Replace(',', '.') : text;

    private static void InsertText(TextBox textBox, string text)
    {
        var start = textBox.SelectionStart;
        textBox.Text = GetProposedText(textBox, text);
        textBox.SelectionStart = start + text.Length;
    }

    private static string GetProposedText(TextBox textBox, string insertedText)
    {
        var start = textBox.SelectionStart;
        var length = textBox.SelectionLength;
        return textBox.Text.Remove(start, length).Insert(start, insertedText);
    }

    private static bool IsValid(TextBox textBox, string proposedText)
    {
        var maxLength = GetMaxLength(textBox);
        if (maxLength > 0 && proposedText.Length > maxLength)
        {
            return false;
        }

        return GetMode(textBox) switch
        {
            NumericInputMode.Integer => Regex.IsMatch(proposedText, @"^\d*$"),
            NumericInputMode.Decimal => Regex.IsMatch(proposedText, $@"^\d*(\.\d{{0,{GetMaxDecimalPlaces(textBox)}}})?$"),
            _ => true
        };
    }
}
