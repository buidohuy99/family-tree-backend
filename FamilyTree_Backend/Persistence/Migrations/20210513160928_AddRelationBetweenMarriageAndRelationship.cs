using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class AddRelationBetweenMarriageAndRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marriage_Relationship_Id",
                table: "Marriage");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentRelationship_OfMarriage",
                table: "Marriage",
                column: "Id",
                principalTable: "Relationship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentRelationship_OfMarriage",
                table: "Marriage");

            migrationBuilder.AddForeignKey(
                name: "FK_Marriage_Relationship_Id",
                table: "Marriage",
                column: "Id",
                principalTable: "Relationship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
