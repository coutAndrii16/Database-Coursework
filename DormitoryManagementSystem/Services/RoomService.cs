using DormitoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Services
{
    public class RoomService
    {
        private readonly DatabaseContext _db;

        public RoomService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<UserInfo>> GetRoomMatesAsync(int userId)
        {
            // спочатку знайти RoomPlaceId для поточного користувача
            var me = await _db.Users
                             .Where(u => u.Id == userId)
                             .Select(u => new { u.RoomPlaceId, u.RoomPlace.RoomId })
                             .FirstOrDefaultAsync();
            if (me?.RoomPlaceId == null) return new List<UserInfo>();

            // тепер знайти всіх, хто має ту ж RoomPlace.RoomId
            return await _db.Users
                .Include(u => u.Faculty)
                .Where(u => u.RoomPlace.RoomId == me.RoomId)
                .ToListAsync();
        }
    }

}
