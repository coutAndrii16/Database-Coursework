using DormitoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Services
{
    public class AdminMessageService
    {
        private readonly DatabaseContext _db;
        public AdminMessageService(DatabaseContext db) => _db = db;

        public async Task<List<AdminMessage>> GetAllActiveAsync()
        {
            return await _db.AdminMessages
                            .Where(m => m.IsActive)
                            .OrderByDescending(m => m.CreatedAt)
                            .ToListAsync();
        }
    }
}
