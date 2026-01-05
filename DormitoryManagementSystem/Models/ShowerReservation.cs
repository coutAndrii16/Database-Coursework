using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Models
{
    // Запис користувача в душ
    public class ShowerReservation
    {
        public int Id { get; set; }

        public int SlotId { get; set; }
        public virtual ShowerSlot Slot { get; set; } = null!;

        public int UserId { get; set; }
        public virtual UserInfo User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
