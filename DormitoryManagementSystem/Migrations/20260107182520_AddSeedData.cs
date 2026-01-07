using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DormitoryManagementSystem.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Додаємо тільки нові записи — без колонки IsDeleted
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Comments", "DormitoryId", "Floor", "Name", "PlacesCount" },
                values: new object[] { 2, "Тестова кімната 2", 1, 1, "11/1", 4 });

            migrationBuilder.InsertData(
                table: "RoomPlaces",
                columns: new[] { "Id", "RoomId" },
                values: new object[,]
                {
                    { 3, 2 },
                    { 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Benefits", "Course", "Email", "FacultyId", "FormOfEducation", "FullName", "Gender", "Group", "IsAdmin", "IsDeleted", "IsLivingInDormitory", "Notes", "PasswordHash", "PhoneNumber", "RoomPlaceId" },
                values: new object[,]
                {
                    { 4, null, 1, "resident3@student.ztu.edu.ua", 3, "Денна", "Тестова Резидентка3", "Жіноча", "ІПЗ-25-0", false, null, true, null, "resident3123", "+380978987654", 3 },
                    { 5, null, 1, "resident4@student.ztu.edu.ua", 3, "Денна", "Тестова Резидентка4", "Жіноча", "ІПЗ-25-0", false, null, true, null, "resident4123", "+380937945692", 4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Видаляємо тільки нові дані
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
