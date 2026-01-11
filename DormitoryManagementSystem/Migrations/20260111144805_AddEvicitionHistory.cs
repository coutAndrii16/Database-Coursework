using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEvicitionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvictionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    EvictionDate = table.Column<DateTime>(nullable: false),
                    OldRoomPlaceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvictionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvictionHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"
        CREATE TRIGGER trg_EvictionLog
        ON Users
        AFTER UPDATE
        AS
        BEGIN
            IF UPDATE(IsLivingInDormitory)
            BEGIN
                INSERT INTO EvictionHistories (UserId, EvictionDate, OldRoomPlaceId)
                SELECT 
                    i.Id,
                    GETDATE(),
                    d.RoomPlaceId
                FROM inserted i
                JOIN deleted d ON i.Id = d.Id
                WHERE 
                    i.IsLivingInDormitory = 0
                    AND d.IsLivingInDormitory = 1;
            END
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_EvictionLog");

            migrationBuilder.DropTable(
                name: "EvictionHistories");
        }

    }
}
