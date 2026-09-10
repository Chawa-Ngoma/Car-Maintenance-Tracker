using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarMaintenanceTracker.Models;

/// <summary>
/// Base class for models that need change notification and per-property
/// validation errors for WPF binding (Validation.HasError / ErrorTemplate).
/// Shared here so Vehicle and ServiceEntry don't each reimplement the same
/// INotifyPropertyChanged/INotifyDataErrorInfo plumbing.
/// </summary>
public abstract class ValidatableModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errorsByProperty = new();

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _errorsByProperty.Count > 0;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !_errorsByProperty.TryGetValue(propertyName, out var errors))
        {
            return Array.Empty<string>();
        }

        return errors;
    }

    /// <summary>
    /// Shared rule for free-text fields that allow digits as part of the text
    /// (e.g. "A4", "10,000 mile service") but shouldn't accept a value that's
    /// nothing but digits. Returns null (no error) for an empty value -- that's
    /// the required-field rule's job, not this one's.
    /// </summary>
    protected static string? ValidateNotPurelyNumeric(string value, string fieldName) =>
        !string.IsNullOrEmpty(value) && value.All(char.IsDigit)
            ? $"{fieldName} cannot be only numbers."
            : null;

    protected void SetField<T>(ref T field, T value, string? error, [CallerMemberName] string? propertyName = null)
    {
        field = value;
        SetError(propertyName!, error);
        OnPropertyChanged(propertyName);
    }

    private void SetError(string propertyName, string? error)
    {
        if (error is null)
        {
            if (_errorsByProperty.Remove(propertyName))
            {
                RaiseErrorsChanged(propertyName);
            }

            return;
        }

        if (!_errorsByProperty.TryGetValue(propertyName, out var errors))
        {
            errors = new List<string>();
            _errorsByProperty[propertyName] = errors;
        }

        if (!errors.Contains(error))
        {
            errors.Clear();
            errors.Add(error);
        }

        RaiseErrorsChanged(propertyName);
    }

    private void RaiseErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
    }

    private void OnPropertyChanged(string? propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
