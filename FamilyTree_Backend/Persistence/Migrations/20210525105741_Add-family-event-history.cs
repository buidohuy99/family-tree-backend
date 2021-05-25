using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class Addfamilyeventhistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamilyEventHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyEventId = table.Column<long>(type: "bigint", nullable: false),
                    PointInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Repeat = table.Column<int>(type: "int", nullable: false),
                    ReminderOffest = table.Column<int>(type: "int", nullable: true),
                    ApplyToFollowingEvents = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyEventHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyEventHistory_FamilyEvent_FamilyEventId",
                        column: x => x.FamilyEventId,
                        principalTable: "FamilyEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyEventHistory_FamilyEventId",
                table: "FamilyEventHistory",
                column: "FamilyEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyEventHistory");
        }
    }
}
