using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PonziWorld.Resources;

internal class InverseBooleanToVisibilityConverter : IValueConverter
{
    public virtual object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture) =>
        value is bool boolValue && boolValue ? Visibility.Collapsed : Visibility.Visible;

    public virtual object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture) =>
        value is Visibility visibility && visibility == Visibility.Visible;
}
