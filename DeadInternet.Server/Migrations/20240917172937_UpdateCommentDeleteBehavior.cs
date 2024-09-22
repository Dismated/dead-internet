using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeadInternet.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7fda689f-1b97-4f34-a80f-23c3fede00b7"
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff10e1f0-693c-4f3e-aa1e-4a3ce2138d8b"
            );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4075c16c-22f6-45f4-bddf-b0ab2a7b6b5c", null, "Admin", "ADMIN" },
                    { "9c1ba571-fd7e-4980-8e97-55272b918fd9", null, "User", "USER" },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4075c16c-22f6-45f4-bddf-b0ab2a7b6b5c"
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c1ba571-fd7e-4980-8e97-55272b918fd9"
            );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7fda689f-1b97-4f34-a80f-23c3fede00b7", null, "Admin", "ADMIN" },
                    { "ff10e1f0-693c-4f3e-aa1e-4a3ce2138d8b", null, "User", "USER" },
                }
            );
        }
    }
}
