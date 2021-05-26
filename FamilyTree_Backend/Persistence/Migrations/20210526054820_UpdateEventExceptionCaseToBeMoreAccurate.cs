using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class UpdateEventExceptionCaseToBeMoreAccurate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderOffest",
                table: "FamilyEventExceptionCases");

            migrationBuilder.DropColumn(
                name: "Repeat",
                table: "FamilyEventExceptionCases");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "FamilyEvent",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReminderOffest",
                table: "FamilyEventExceptionCases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Repeat",
                table: "FamilyEventExceptionCases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "FamilyEvent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
