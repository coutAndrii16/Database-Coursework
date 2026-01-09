using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomPlaceSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlaceNumber",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 2,
                column: "PlaceNumber",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 3,
                column: "PlaceNumber",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 4,
                column: "PlaceNumber",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlaceNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 2,
                column: "PlaceNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 3,
                column: "PlaceNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 4,
                column: "PlaceNumber",
                value: 0);
        }
    }
}
