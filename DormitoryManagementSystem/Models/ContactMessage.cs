using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public int? UserId { get; set; }      // null → анонім
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        // навігація, якщо треба (для юзер імені)
        public UserInfo? User { get; set; }
    }

}
