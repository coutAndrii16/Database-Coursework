using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBusyToRoomPlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBusy",
                table: "RoomPlaces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsBusy",
                value: true);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsBusy",
                value: true);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsBusy",
                value: true);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsBusy",
                value: true);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsBusy",
                value: true);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsBusy",
                value: false);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsBusy",
                value: false);

            migrationBuilder.UpdateData(
                table: "RoomPlaces",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsBusy",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBusy",
                table: "RoomPlaces");
        }
    }
}
