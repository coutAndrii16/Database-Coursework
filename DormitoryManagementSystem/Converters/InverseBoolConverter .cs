using System;
using System.Globalization;
using System.Windows.Data;

namespace DormitoryManagementSystem.Converters
{
    /// <summary>
    /// Перетворює bool → !bool (використовується для IsEnabled="{Binding IsRead, Converter={StaticResource InverseBoolConverter}}")
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
