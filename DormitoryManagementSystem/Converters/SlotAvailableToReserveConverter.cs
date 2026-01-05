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
        public class SlotAvailableToReserveConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is ShowerSlot slot)
                {
                    // Обчислюємо повний час закінчення слота
                    DateTime endDateTime = slot.Date + slot.EndTime;

                    // Якщо час слота вже минув — він неактивний
                    if (endDateTime < DateTime.Now)
                        return false;

                    // Інакше — активний
                    return true;
                }

                return false; // null або неправильний тип
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotImplementedException();
        }
    }
