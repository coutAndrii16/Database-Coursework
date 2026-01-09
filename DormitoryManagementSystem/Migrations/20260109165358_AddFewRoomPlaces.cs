using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFewRoomPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoomPlaces",
                columns: new[] { "Id", "PlaceNumber", "RoomId" },
                values: new object[,]
                {
                    { 5, null, 2 },
                    { 6, null, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
