using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class AddFamilyMemory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyEvents_FamilyTree_FamilyTreeId",
                table: "FamilyEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyEvents",
                table: "FamilyEvents");

            migrationBuilder.RenameTable(
                name: "FamilyEvents",
                newName: "FamilyEvent");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyEvents_FamilyTreeId",
                table: "FamilyEvent",
                newName: "IX_FamilyEvent_FamilyTreeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyEvent",
                table: "FamilyEvent",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FamilyMemory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyTreeId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemoryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMemory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyMemory_FamilyTree_FamilyTreeId",
                        column: x => x.FamilyTreeId,
                        principalTable: "FamilyTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMemory_FamilyTreeId",
                table: "FamilyMemory",
                column: "FamilyTreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyEvent_FamilyTree_FamilyTreeId",
                table: "FamilyEvent",
                column: "FamilyTreeId",
                principalTable: "FamilyTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyEvent_FamilyTree_FamilyTreeId",
                table: "FamilyEvent");

            migrationBuilder.DropTable(
                name: "FamilyMemory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyEvent",
                table: "FamilyEvent");

            migrationBuilder.RenameTable(
                name: "FamilyEvent",
                newName: "FamilyEvents");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyEvent_FamilyTreeId",
                table: "FamilyEvents",
                newName: "IX_FamilyEvents_FamilyTreeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyEvents",
                table: "FamilyEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyEvents_FamilyTree_FamilyTreeId",
                table: "FamilyEvents",
                column: "FamilyTreeId",
                principalTable: "FamilyTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
