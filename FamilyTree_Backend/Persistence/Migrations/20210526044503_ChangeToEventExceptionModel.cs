using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class ChangeToEventExceptionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyEventHistory");

            migrationBuilder.AlterColumn<int>(
                name: "Repeat",
                table: "FamilyEvent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "ParentEventId",
                table: "FamilyEvent",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FamilyEventExceptionCases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyEventId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Repeat = table.Column<int>(type: "int", nullable: false),
                    ReminderOffest = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyEventExceptionCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyEventExceptionCases_FamilyEvent_FamilyEventId",
                        column: x => x.FamilyEventId,
                        principalTable: "FamilyEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyEvent_ParentEventId",
                table: "FamilyEvent",
                column: "ParentEventId",
                unique: true,
                filter: "[ParentEventId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyEventExceptionCases_FamilyEventId",
                table: "FamilyEventExceptionCases",
                column: "FamilyEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyEvent_FamilyEvent_ParentEventId",
                table: "FamilyEvent",
                column: "ParentEventId",
                principalTable: "FamilyEvent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyEvent_FamilyEvent_ParentEventId",
                table: "FamilyEvent");

            migrationBuilder.DropTable(
                name: "FamilyEventExceptionCases");

            migrationBuilder.DropIndex(
                name: "IX_FamilyEvent_ParentEventId",
                table: "FamilyEvent");

            migrationBuilder.DropColumn(
                name: "ParentEventId",
                table: "FamilyEvent");

            migrationBuilder.AlterColumn<int>(
                name: "Repeat",
                table: "FamilyEvent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FamilyEventHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplyToFollowingEvents = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FamilyEventId = table.Column<long>(type: "bigint", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReminderOffest = table.Column<int>(type: "int", nullable: true),
                    Repeat = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
    }
}
