using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LLMForum.Server.Migrations
{
    /// <inheritdoc />
    public partial class @default : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "What is this question?",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "What is this question?");
        }
    }
}
