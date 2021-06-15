using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class AddMoreLogInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestPath",
                table: "RequestResponseLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RequestResponseLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestPath",
                table: "RequestResponseLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RequestResponseLogs");
        }
    }
}
