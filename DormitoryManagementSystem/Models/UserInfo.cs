using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DormitoryManagementSystem.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; } // Пошта для входу
        public string PasswordHash { get; set; } // Захешований пароль(в майбутньому)
        public bool IsAdmin { get; set; } // Чи адмін користувач
        public bool IsLivingInDormitory { get; set; } // Чи проживає у гуртожитку (для студентів)
        // Додаткові дані для студентів
        public string? Group { get; set; }
        public int? Course { get; set; }
        public string? PhoneNumber { get; set; } // Номер телефону
        public string? FormOfEducation { get; set; }
        public int? FacultyId { get; set; }
        public Faculty? Faculty { get; set; }
        public int? RoomPlaceId { get; set; } // Посилання на місце в кімнаті (якщо живе)
        public RoomPlace? RoomPlace { get; set; }
        public string? Gender { get; set; }
        public string? Benefits { get; set; } // Пільги
        public string? Notes { get; set; }
        public bool? IsDeleted { get; set; }
        // Конструктор без параметрів (обов'язковий для EF або ініціалізації)
        public UserInfo() { }
    }
}        //left add new borders which'll be get into api