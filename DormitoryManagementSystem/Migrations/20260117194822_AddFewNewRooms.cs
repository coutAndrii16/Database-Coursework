using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFewNewRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Floor", "PlacesCount", "DormitoryId", "Comments" },
                values: new object[,]
                {
                    { 4, "42/1", 4, 4, 1, "Тестова кімната 4" },
                    { 5, "42/2", 4, 2, 1, "Тестова кімната 5" }
                });

            migrationBuilder.InsertData(
                table: "RoomPlaces",
                columns: new[] { "Id", "RoomId" },
                values: new object[,]
                {
                    { 9, 4 },
                    { 10, 4 },
                    { 11, 4 },
                    { 12, 4 },
                    { 13, 5 },
                    { 14, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValues: new object[] { 9, 10, 11, 12, 13, 14 });

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValues: new object[] { 4, 5 });
        }
    }
}
