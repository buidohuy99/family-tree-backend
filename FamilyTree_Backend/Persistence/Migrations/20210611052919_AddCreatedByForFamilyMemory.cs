using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class AddCreatedByForFamilyMemory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserID",
                table: "FamilyMemory",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMemory_CreatedByUserID",
                table: "FamilyMemory",
                column: "CreatedByUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMemory_AspNetUsers_CreatedByUserID",
                table: "FamilyMemory",
                column: "CreatedByUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMemory_AspNetUsers_CreatedByUserID",
                table: "FamilyMemory");

            migrationBuilder.DropIndex(
                name: "IX_FamilyMemory_CreatedByUserID",
                table: "FamilyMemory");

            migrationBuilder.DropColumn(
                name: "CreatedByUserID",
                table: "FamilyMemory");
        }
    }
}
