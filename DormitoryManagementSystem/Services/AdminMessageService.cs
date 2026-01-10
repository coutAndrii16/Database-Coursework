using DormitoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Services
{
    public class AdminMessageService
    {
        private readonly DatabaseContext _db;
        
        public AdminMessageService(DatabaseContext db) => _db = db;

        /// <summary>
        /// Отримати всі активні оголошення (для показу резидентам)
        /// </summary>
        public async Task<List<AdminMessage>> GetAllActiveAsync()
        {
            return await _db.AdminMessages
                .Where(m => m.IsActive)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Отримати поточне активне оголошення (найновіше)
        /// </summary>
        public async Task<AdminMessage?> GetCurrentActiveAsync()
        {
            return await _db.AdminMessages
                .Where(m => m.IsActive)
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Отримати всі оголошення (включно з неактивними)
        /// </summary>
        public async Task<List<AdminMessage>> GetAllAsync()
        {
            return await _db.AdminMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Створити нове оголошення
        /// </summary>
        public async Task CreateMessageAsync(string title, string content)
        {
            var message = new AdminMessage
            {
                Title = title,
                Content = content,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            
            _db.AdminMessages.Add(message);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Деактивувати оголошення (видалити зі списку активних)
        /// </summary>
        public async Task DeactivateMessageAsync(int messageId)
        {
            var msg = await _db.AdminMessages.FindAsync(messageId);
            if (msg != null)
            {
                msg.IsActive = false;
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Видалити оголошення повністю з БД
        /// </summary>
        public async Task DeleteMessageAsync(int messageId)
        {
            var msg = await _db.AdminMessages.FindAsync(messageId);
            if (msg != null)
            {
                _db.AdminMessages.Remove(msg);
                await _db.SaveChangesAsync();
            }
        }
    }
}