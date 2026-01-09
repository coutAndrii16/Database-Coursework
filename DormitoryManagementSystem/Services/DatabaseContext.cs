using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DormitoryManagementSystem.Models;

namespace DormitoryManagementSystem.Services
{
    public class DatabaseContext : DbContext
    {
            public DbSet<Dormitory> Dormitories { get; set; }
            public DbSet<Room> Rooms { get; set; }
            public DbSet<RoomPlace> RoomPlaces { get; set; }
            public DbSet<UserInfo> Users { get; set; }
            public DbSet<Faculty> Faculties { get; set; }
        public DbSet<AdminMessage> AdminMessages { get; set; } = null!;
        public DbSet<ShowerSlot> ShowerSlots { get; set; } = null!;
        public DbSet<ShowerReservation> ShowerReservations { get; set; } = null!;
        public DbSet<ContactMessage> ContactMessages { get; set; }


        public DatabaseContext() { }
            public DatabaseContext(DbContextOptions<DatabaseContext> options)
                : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {//встав юзера
            base.OnModelCreating(modelBuilder); // Не забувай
             //  Конфігурація зв’язку 1:1 між RoomPlace та UserInfo
            modelBuilder.Entity<RoomPlace>()
                .HasOne(rp => rp.Student)
                .WithOne(u => u.RoomPlace)
                .HasForeignKey<UserInfo>(u => u.RoomPlaceId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<RoomPlace>()
                .HasOne(rp => rp.Room)
                .WithMany(r => r.Places)
                .HasForeignKey(rp => rp.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AdminMessage>()
               .Property(m => m.CreatedAt)
               .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ShowerReservation>()
                .HasIndex(r => new { r.UserId, r.SlotId })
                .IsUnique(); // один користувач — один слот
            // Факультети
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty { Id = 1, Name = "Факультет бізнесу та сфери обслуговування" },
                new Faculty { Id = 2, Name = "Факультет гірничої справи, природокористування та будівництва" },
                new Faculty { Id = 3, Name = "Факультет інформаційно-комп'ютерних технологій" },
                new Faculty { Id = 4, Name = "Факультет комп'ютерно-інтегрованих технологій, мехатроніки і робототехніки" },
                new Faculty { Id = 5, Name = "Факультет національної безпеки, права та міжнародних відносин" },
                new Faculty { Id = 6, Name = "Факультет педагогічних технологій та освіти впродовж життя" }
//                new Faculty { Id = 7, Name = "ФКН" }
            );
            // Гуртожиток
            modelBuilder.Entity<Dormitory>().HasData(new Dormitory
            {
                Id = 1,
                Name = "Гуртожиток №1"
            });

            // Кімната
            modelBuilder.Entity<Room>().HasData(new Room
            {
                Id = 1,
                Name = "11/3",
                Floor = 1,
                PlacesCount = 2,
                DormitoryId = 1,
                Comments = "Тестова кімната"
            },
                new Room
                {
                    Id = 2,
                    Name = "11/1",
                    Floor = 1,
                    PlacesCount = 4,
                    DormitoryId = 1,
                    Comments = "Тестова кімната 2"
                },
                new Room
                {
                    Id = 3,
                    Name = "11/2",
                    Floor = 1,
                    PlacesCount = 2,
                    DormitoryId = 1,
                    Comments = "Тестова кімната 3"
                });

            // Місця
            modelBuilder.Entity<RoomPlace>().HasData(
                new RoomPlace { Id = 1, RoomId = 1, PlaceNumber = 1},
                new RoomPlace { Id = 2, RoomId = 1, PlaceNumber = 2},
                new RoomPlace { Id = 3, RoomId = 2, PlaceNumber = 1},
                new RoomPlace { Id = 4, RoomId = 2, PlaceNumber = 2},
                new RoomPlace { Id = 5, RoomId = 2},
                new RoomPlace { Id = 6, RoomId = 2},
                new RoomPlace { Id = 7, RoomId = 3},
                new RoomPlace { Id = 8, RoomId = 3}
            );

            // Адмін
            modelBuilder.Entity<UserInfo>().HasData(new UserInfo
            {
                Id = 1,
                FullName = "Тестовий Адмін",
                Email = "admin@ztu.edu.ua",
                PasswordHash = "admin123",
                IsAdmin = true,
                IsLivingInDormitory = false,
                Group = null,
                Course = null,
                PhoneNumber = "+380991112233",
                FormOfEducation = null,
                FacultyId = null,
                RoomPlaceId = null,
                Gender = null
            },

            // Резидент
           new UserInfo
            {
                Id = 2,
                FullName = "Тестова Резидентка",
                Email = "resident@student.ztu.edu.ua",
                PasswordHash = "resident123",
                IsAdmin = false,
                IsLivingInDormitory = true,
                Group = "ІПЗ-23-0",
                Course = 2,
                PhoneNumber = "+380991112244",
                FormOfEducation = "Денна",
                FacultyId = 3,
                RoomPlaceId = 1,
                Gender = "Жіноча"
            },
           // Резидент2
           new UserInfo
            {
                Id = 3,
                FullName = "Тестова Резидентка2",
                Email = "resident2@student.ztu.edu.ua",
                PasswordHash = "resident2123",
                IsAdmin = false,
                IsLivingInDormitory = true,
                Group = "ІПЗ-24-0",
                Course = 1,
                PhoneNumber = "+380966879654",
                FormOfEducation = "Денна",
                FacultyId = 3,
                RoomPlaceId = 2,
                Gender = "Жіноча"
            },
            // Резидент3
            new UserInfo
                {
                    Id = 4,
                    FullName = "Тестова Резидентка3",
                    Email = "resident3@student.ztu.edu.ua",
                    PasswordHash = "resident3123",
                    IsAdmin = false,
                    IsLivingInDormitory = true,
                    Group = "ІПЗ-25-0",
                    Course = 1,
                    PhoneNumber = "+380978987654",
                    FormOfEducation = "Денна",
                    FacultyId = 3,
                    RoomPlaceId = 3,
                    Gender = "Жіноча"
                },
            // Резидент4
            new UserInfo
                {
                    Id = 5,
                    FullName = "Тестова Резидентка4",
                    Email = "resident4@student.ztu.edu.ua",
                    PasswordHash = "resident4123",
                    IsAdmin = false,
                    IsLivingInDormitory = true,
                    Group = "ІПЗ-25-0",
                    Course = 1,
                    PhoneNumber = "+380937945692",
                    FormOfEducation = "Денна",
                    FacultyId = 3,
                    RoomPlaceId = 4,
                    Gender = "Жіноча"
                }
           );
            modelBuilder.Entity<AdminMessage>().HasData(
            new AdminMessage
            {
            Id = 1,
            Title = "Екстрене повідомлення",
            Content = "Завтра відключення води з 9:00 до 18:00.",
            IsActive = true,
            CreatedAt = new DateTime(2025, 5, 19, 0, 0, 0)
            }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Підстав своє ім'я сервера та бази
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DormitoryDB;Trusted_Connection=True; TrustServerCertificate=True;");
            }
            }
    }
}
