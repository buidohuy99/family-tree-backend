using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class ClarifyReferenceToFamilyTree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Constraints_FamiliesOfTree",
                table: "Family");

            migrationBuilder.DropForeignKey(
                name: "Constraints_PeopleOfTree",
                table: "Person");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyOfTree",
                table: "Family",
                column: "FamilyTreeId",
                principalTable: "FamilyTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonOfTree",
                table: "Person",
                column: "FamilyTreeId",
                principalTable: "FamilyTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyOfTree",
                table: "Family");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonOfTree",
                table: "Person");

            migrationBuilder.AddForeignKey(
                name: "Constraints_FamiliesOfTree",
                table: "Family",
                column: "FamilyTreeId",
                principalTable: "FamilyTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Constraints_PeopleOfTree",
                table: "Person",
                column: "FamilyTreeId",
                principalTable: "FamilyTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
