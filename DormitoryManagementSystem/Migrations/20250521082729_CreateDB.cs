using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DormitoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dormitories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dormitories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    PlacesCount = table.Column<int>(type: "int", nullable: false),
                    DormitoryId = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Dormitories_DormitoryId",
                        column: x => x.DormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomPlaces_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsLivingInDormitory = table.Column<bool>(type: "bit", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormOfEducation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<int>(type: "int", nullable: true),
                    RoomPlaceId = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_RoomPlaces_RoomPlaceId",
                        column: x => x.RoomPlaceId,
                        principalTable: "RoomPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Dormitories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Гуртожиток №1" });

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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Benefits", "Course", "Email", "FacultyId", "FormOfEducation", "FullName", "Gender", "Group", "IsAdmin", "IsLivingInDormitory", "Notes", "PasswordHash", "PhoneNumber", "RoomPlaceId" },
                values: new object[] { 1, null, null, "admin@ztu.edu.ua", null, null, "Тестовий Адмін", null, null, true, false, null, "admin123", "+380991112233", null });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Comments", "DormitoryId", "Floor", "Name", "PlacesCount" },
                values: new object[] { 1, "Тестова кімната", 1, 1, "11/3", 2 });

            migrationBuilder.InsertData(
                table: "RoomPlaces",
                columns: new[] { "Id", "RoomId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Benefits", "Course", "Email", "FacultyId", "FormOfEducation", "FullName", "Gender", "Group", "IsAdmin", "IsLivingInDormitory", "Notes", "PasswordHash", "PhoneNumber", "RoomPlaceId" },
                values: new object[] { 2, null, 2, "resident@student.ztu.edu.ua", 3, "Денна", "Тестова Резидентка", "Жіноча", "ІПЗ-23-0", false, true, null, "resident123", "+380991112244", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlaces_RoomId",
                table: "RoomPlaces",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DormitoryId",
                table: "Rooms",
                column: "DormitoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FacultyId",
                table: "Users",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoomPlaceId",
                table: "Users",
                column: "RoomPlaceId",
                unique: true,
                filter: "[RoomPlaceId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "RoomPlaces");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Dormitories");
        }
    }
}
