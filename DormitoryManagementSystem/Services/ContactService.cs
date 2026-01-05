using Microsoft.EntityFrameworkCore;
using DormitoryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DormitoryManagementSystem.Services
{
    public class ContactService
    {
        private readonly DatabaseContext _db;
        public ContactService(DatabaseContext db) => _db = db;

        public async Task AddMessageAsync(int? userId, string content)
        {
            var msg = new ContactMessage { UserId = userId, Content = content };
            _db.ContactMessages.Add(msg);

            await _db.SaveChangesAsync();
            Debug.WriteLine($"New Id: {msg.Id}");
        }

        public async Task<List<ContactMessage>> GetAllAsync()
            => await _db.ContactMessages
                        .Include(m => m.User)
                        .OrderByDescending(m => m.CreatedAt)
                        .ToListAsync();

        public async Task MarkAsReadAsync(int messageId)
        {
            var m = await _db.ContactMessages.FindAsync(messageId);
            if (m == null) return;
            m.IsRead = true;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var m = await _db.ContactMessages.FindAsync(messageId);
            if (m == null) return;
            _db.ContactMessages.Remove(m);
            await _db.SaveChangesAsync();
        }
    }

}
