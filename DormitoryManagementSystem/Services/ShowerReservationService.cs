using DormitoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Services
{
    public class ShowerReservationService
    {
        private readonly DatabaseContext _db;

        public ShowerReservationService(DatabaseContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Отримує список слотів на сьогодні з інформацією про резервації
        /// </summary>
        public async Task EnsureSlotsForTodayAsync()
        {
            var today = DateTime.Today;

            // Видаляємо старі слоти перед генерацією
            var old = _db.ShowerSlots.Where(s => s.Date < today).ToList();
            _db.ShowerSlots.RemoveRange(old);
            await _db.SaveChangesAsync();

            // Генеруємо слоти, якщо їх немає на сьогодні
            bool exists = _db.ShowerSlots.Any(s => s.Date == today);
            if (!exists)
            {
                var generator = new ShowerSlotGeneratorService(_db);
                await generator.GenerateSlotsForTodayAsync();
            }
        }

        public async Task<List<ShowerSlot>> GetAvailableSlotsAsync(string gender)
        {
            return await _db.ShowerSlots
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.User)
                .Where(s => s.Date == DateTime.Today)
                .ToListAsync();
        }        /// <summary>
                 /// Робить резервацію користувача на конкретний слот, якщо це можливо
                 /// </summary>
        public async Task<bool> TryAddReservationAsync(int userId, int slotId)
        {
            var today = DateTime.Today;

            var alreadyReserved = await _db.ShowerReservations
                .Include(r => r.Slot)
                .AnyAsync(r => r.UserId == userId && r.Slot.Date == today);

            if (alreadyReserved)
                return false;

            var slot = await _db.ShowerSlots
                .Include(s => s.Reservations)
                .FirstOrDefaultAsync(s => s.Id == slotId);

            if (slot == null || slot.Reservations.Count >= 4)
                return false;
            if (slot.Date == DateTime.Today && slot.EndTime < DateTime.Now.TimeOfDay)
                return false; // Слот уже минув


            _db.ShowerReservations.Add(new ShowerReservation
            {
                SlotId = slotId,
                UserId = userId
            });

            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Скасовує резервацію користувача за ID
        /// </summary>
        public async Task<bool> CancelReservationAsync(int userId, int slotId)
        {
            var reservation = await _db.ShowerReservations
                .FirstOrDefaultAsync(r => r.UserId == userId && r.SlotId == slotId);

            if (reservation == null)
                return false;

            _db.ShowerReservations.Remove(reservation);
            await _db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Перевірка, чи користувач вже має запис на сьогодні
        /// </summary>
        public async Task<bool> HasUserReservedTodayAsync(int userId)
        {
            var today = DateTime.Today;

            return await _db.ShowerReservations
                .Include(r => r.Slot)
                .AnyAsync(r => r.UserId == userId && r.Slot.Date == today);
        }

        /// <summary>
        /// Отримує всі резервації на сьогодні (для адміністратора)
        /// </summary>
        public List<ShowerReservation> GetReservationsForToday()
        {
            var today = DateTime.Today;

            return _db.ShowerReservations
                .Include(r => r.User)
                .Include(r => r.Slot)
                .Where(r => r.Slot.Date == today)
                .ToList();
        }
    }
}
