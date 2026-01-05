using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddShowerReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlaces_Rooms_RoomId",
                table: "RoomPlaces");

            migrationBuilder.CreateTable(
                name: "ShowerSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MaxReservations = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowerSlots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShowerReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlotId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowerReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowerReservations_ShowerSlots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "ShowerSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShowerReservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowerReservations_SlotId",
                table: "ShowerReservations",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowerReservations_UserId_SlotId",
                table: "ShowerReservations",
                columns: new[] { "UserId", "SlotId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlaces_Rooms_RoomId",
                table: "RoomPlaces",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlaces_Rooms_RoomId",
                table: "RoomPlaces");

            migrationBuilder.DropTable(
                name: "ShowerReservations");

            migrationBuilder.DropTable(
                name: "ShowerSlots");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlaces_Rooms_RoomId",
                table: "RoomPlaces",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
