using System;
using System.Globalization;
using System.Windows.Data;

namespace DormitoryManagementSystem.Converters
{
    /// <summary>
    /// Перетворює null → true, non-null → false
    /// (щоб DataGridCheckBoxColumn показував чекбокс “Анонім”)
    /// </summary>
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // не використовується у зворотньому біндингу
            throw new NotImplementedException();
        }
    }
}
