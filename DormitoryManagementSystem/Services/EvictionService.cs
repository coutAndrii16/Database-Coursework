using DormitoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Services
{
    public class EvictionService
    {
        private readonly DatabaseContext _db;

        public EvictionService(DatabaseContext db) => _db = db;

        /// <summary>
        /// Виселити мешканця (ледаче видалення)
        /// </summary>
        public async Task<bool> EvictResidentAsync(int userId, string reason)
        {
            var user = await _db.Users
                .Include(u => u.RoomPlace)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || !user.IsLivingInDormitory)
                return false;

            var evictionDate = DateTime.Now;
            var blockDate = evictionDate.AddDays(7);

            // 1. Оновити статус користувача
            user.IsLivingInDormitory = false;
            user.EvictionDate = evictionDate;
            // IsDeleted залишаємо false — блокування через 7 днів

            // 2. Звільнити місце в кімнаті
            if (user.RoomPlace != null)
            {
                var roomPlace = await _db.RoomPlaces.FindAsync(user.RoomPlaceId);
                if (roomPlace != null)
                {
                    roomPlace.PlaceNumber = null; // Звільнити місце
                }
                user.RoomPlaceId = null;
            }

            // 3. Створити сповіщення для користувача
            var notification = new EvictionNotification
            {
                UserId = userId,
                Reason = reason,
                EvictionDate = evictionDate,
                BlockDate = blockDate,
                IsRead = false
            };
            _db.EvictionNotifications.Add(notification);

            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Отримати сповіщення про виселення для користувача
        /// </summary>
        public async Task<EvictionNotification?> GetEvictionNotificationAsync(int userId)
        {
            return await _db.EvictionNotifications
                .FirstOrDefaultAsync(n => n.UserId == userId && !n.IsRead);
        }

        /// <summary>
        /// Позначити сповіщення як прочитане
        /// </summary>
        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _db.EvictionNotifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Блокувати облікові записи, у яких минуло 7 днів після виселення
        /// (викликати через фоновий сервіс або при вході)
        /// </summary>
        public async Task BlockExpiredAccountsAsync()
        {
            var now = DateTime.Now;

            var expiredUsers = await _db.Users
                .Where(u => u.EvictionDate.HasValue
                            && u.IsDeleted != true
                            && u.EvictionDate.Value.AddDays(7) <= now)
                .ToListAsync();
            
            foreach (var user in expiredUsers)
            {
                user.IsDeleted = true;
            }

            await _db.SaveChangesAsync();
        }
    }
}