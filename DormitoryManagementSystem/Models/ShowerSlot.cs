using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Models
{
    // Модель для черги в душ
    public class ShowerSlot
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } // Дата, до якої належить слот
        public TimeSpan StartTime { get; set; } // Початок інтервалу (наприклад 06:00)
        public TimeSpan EndTime { get; set; }   // Кінець інтервалу (наприклад 07:00)
        public int MaxReservations { get; set; } = 4;
        public string Gender { get; set; } = string.Empty;
        public virtual ICollection<ShowerReservation> Reservations { get; set; } = new List<ShowerReservation>();
    }
}

