using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeadInternet.Server.Migrations
{
    /// <inheritdoc />
    public partial class NoPersonalityYet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AIPersonalities_AIPersonalityId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "AIPersonalities");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AIPersonalityId",
                table: "Posts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4075c16c-22f6-45f4-bddf-b0ab2a7b6b5c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c1ba571-fd7e-4980-8e97-55272b918fd9");

            migrationBuilder.DropColumn(
                name: "AIPersonalityId",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AIPersonalityId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AIPersonalities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIPersonalities", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4075c16c-22f6-45f4-bddf-b0ab2a7b6b5c", null, "Admin", "ADMIN" },
                    { "9c1ba571-fd7e-4980-8e97-55272b918fd9", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AIPersonalityId",
                table: "Posts",
                column: "AIPersonalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AIPersonalities_AIPersonalityId",
                table: "Posts",
                column: "AIPersonalityId",
                principalTable: "AIPersonalities",
                principalColumn: "Id");
        }
    }
}
