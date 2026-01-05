using DormitoryManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace DormitoryManagementSystem.Converters
{
    public class IsCurrentUserToCommandConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int? reservedBy = values[0] as int?;
            int currentUserId = int.Parse(parameter.ToString());
            var root = values[1] as FrameworkElement;
            var vm = root?.DataContext as ShowerReservationViewModel;

            if (reservedBy == currentUserId)
                return vm?.CancelReservationCommand;
            else
                return vm?.ReserveSlotCommand;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

}
