using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class RemoveMarriage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marriage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Marriage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DateOfMarriage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndOfMarriage = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marriage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentRelationship_OfMarriage",
                        column: x => x.Id,
                        principalTable: "Relationship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
