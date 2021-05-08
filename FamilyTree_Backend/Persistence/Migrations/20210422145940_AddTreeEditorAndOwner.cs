using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class AddTreeEditorAndOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "FamilyTree",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FamilyTreeId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamilyTree_OwnerId",
                table: "FamilyTree",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyTreeId",
                table: "AspNetUsers",
                column: "FamilyTreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FamilyTree_FamilyTreeId",
                table: "AspNetUsers",
                column: "FamilyTreeId",
                principalTable: "FamilyTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyTree_AspNetUsers_OwnerId",
                table: "FamilyTree",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FamilyTree_FamilyTreeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyTree_AspNetUsers_OwnerId",
                table: "FamilyTree");

            migrationBuilder.DropIndex(
                name: "IX_FamilyTree_OwnerId",
                table: "FamilyTree");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyTreeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "FamilyTree");

            migrationBuilder.DropColumn(
                name: "FamilyTreeId",
                table: "AspNetUsers");
        }
    }
}
