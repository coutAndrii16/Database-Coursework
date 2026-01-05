using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.Migrations
{
    public partial class CorrectSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Faculties
            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Факультет бізнесу та сфери обслуговування" },
                    { 2, "Факультет гірничої справи, природокористування та будівництва" },
                    { 3, "Факультет інформаційно-комп'ютерних технологій" },
                    { 4, "Факультет комп'ютерно-інтегрованих технологій, мехатроніки і робототехніки" },
                    { 5, "Факультет національної безпеки, права та міжнародних відносин" },
                    { 6, "Факультет педагогічних технологій та освіти впродовж життя" }
                });

            // Dormitory
            migrationBuilder.InsertData(
                table: "Dormitories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Гуртожиток №1" });

            // Room
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Floor", "PlacesCount", "DormitoryId", "Comments" },
                values: new object[] { 1, "11/3", 1, 2, 1, "Тестова кімната" });

            // RoomPlaces
            migrationBuilder.InsertData(
                table: "RoomPlaces",
                columns: new[] { "Id", "RoomId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 }
                });

            // Users
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]
                {
                    "Id",
                    "FullName",
                    "Email",
                    "PasswordHash",
                    "IsAdmin",
                    "IsLivingInDormitory",
                    "Group",
                    "Course",
                    "PhoneNumber",
                    "FormOfEducation",
                    "FacultyId",
                    "RoomPlaceId",
                    "Gender"
                },
                values: new object[,]
                {
                    { 1, "Тестовий Адмін", "admin@ztu.edu.ua", "admin123", true, false, null, null, "+380991112233", null, null, null, null },
                    { 2, "Тестова Резидентка", "resident@student.ztu.edu.ua", "resident123", false, true, "ІПЗ-23-0", 2, "+380991112244", "Денна", 3, 1, "Жіноча" },
                    { 3, "Тестова Резидентка2", "resident2@student.ztu.edu.ua", "resident2123", false, true, "ІПЗ-24-0", 1, "+380966879654", "Денна", 3, 2, "Жіноча" }
                });

            // AdminMessage
            migrationBuilder.InsertData(
                table: "AdminMessages",
                columns: new[] { "Id", "Title", "Content", "IsActive", "CreatedAt" },
                values: new object[]
                {
                    1,
                    "Екстрене повідомлення",
                    "Завтра відключення води з 9:00 до 18:00.",
                    true,
                    new DateTime(2025, 5, 19, 0, 0, 0, DateTimeKind.Unspecified)
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // AdminMessage
            migrationBuilder.DeleteData(
                table: "AdminMessages",
                keyColumn: "Id",
                keyValue: 1);

            // Users
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });

            // RoomPlaces
            migrationBuilder.DeleteData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2 });

            // Rooms
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            // Dormitories
            migrationBuilder.DeleteData(
                table: "Dormitories",
                keyColumn: "Id",
                keyValue: 1);

            // Faculties
            migrationBuilder.DeleteData(
                table: "Faculties",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6 });
        }
    }
}
