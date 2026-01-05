using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Models
{
    public class ShowerSlotWithReservation
    {
        public ShowerSlot Slot { get; set; }
        public ShowerReservation? Reservation { get; set; } // Чи зайнятий слот — для кнопки
    }

}
