using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DormitoryManagementSystem.Converters
{
    public class SlotReservationColorMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] — зарезервовано ким (UserId)
            // values[1] — поточний користувач (UserId)
            int reservedByUserId = 0;
            int currentUserId = 0;

            if (values.Length > 0 && values[0] is int r)
                reservedByUserId = r;
            if (values.Length > 1 && values[1] is int c)
                currentUserId = c;

            if (reservedByUserId == 0)
                return Brushes.Green;
            if (reservedByUserId == currentUserId)
                return Brushes.Blue;
            return Brushes.Red;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
