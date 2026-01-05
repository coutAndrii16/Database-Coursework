using DormitoryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DormitoryManagementSystem.Converters
{
    public class SlotButtonTextMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var reservation = values[0] as ShowerReservation;
            var userId = values[1] as int?;

            if (reservation == null)
                return "Записатись";

            if (reservation.UserId == userId)
                return "Скасувати";

            return "Зайнято";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
