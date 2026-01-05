using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Benefits", "Course", "Email", "FacultyId", "FormOfEducation", "FullName", "Gender", "Group", "IsAdmin", "IsLivingInDormitory", "Notes", "PasswordHash", "PhoneNumber", "RoomPlaceId" },
                values: new object[] { 3, null, 1, "resident2@student.ztu.edu.ua", 3, "Денна", "Тестова Резидентка2", "Жіноча", "ІПЗ-24-0", false, true, null, "resident2123", "+380966879654", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
