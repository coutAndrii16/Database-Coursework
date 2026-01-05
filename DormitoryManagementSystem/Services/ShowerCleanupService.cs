using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Services
{// Фоновий процес (можна реалізувати як ScheduledService у майбутньому)
    public static class ShowerCleanupService
    {
        public static void CleanupSlots()
        {
            using var db = new DatabaseContext();
            var today = DateTime.Today;

            var old = db.ShowerSlots.Where(s => s.Date < today);
            db.ShowerSlots.RemoveRange(old);

            // Щоб очищати резервації на сьогодні:
            var todaySlots = db.ShowerSlots.Include(s => s.Reservations).Where(s => s.Date == today);
            foreach (var slot in todaySlots)
            {
                slot.Reservations.Clear();
            }

            db.SaveChanges();
        }
    }
}
