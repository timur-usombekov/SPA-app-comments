using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPA_app_comments.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removephotofield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Comments",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
