using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFewPlaceNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 6,
                column: "PlaceNumber",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 7,
                column: "PlaceNumber",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 6,
                column: "PlaceNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 7,
                column: "PlaceNumber",
                value: null);
        }
    }
}
