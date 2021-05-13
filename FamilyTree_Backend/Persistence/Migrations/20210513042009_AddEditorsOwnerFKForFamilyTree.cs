using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    public partial class AddEditorsOwnerFKForFamilyTree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FamilyTree_FamilyTreeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyTree_AspNetUsers_OwnerId",
                table: "FamilyTree");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyTreeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyTreeId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserFamilyTree",
                columns: table => new
                {
                    EditorOfFamilyTreesId = table.Column<long>(type: "bigint", nullable: false),
                    EditorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserFamilyTree", x => new { x.EditorOfFamilyTreesId, x.EditorsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserFamilyTree_AspNetUsers_EditorsId",
                        column: x => x.EditorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserFamilyTree_FamilyTree_EditorOfFamilyTreesId",
                        column: x => x.EditorOfFamilyTreesId,
                        principalTable: "FamilyTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserFamilyTree_EditorsId",
                table: "ApplicationUserFamilyTree",
                column: "EditorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerOfTree",
                table: "FamilyTree",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnerOfTree",
                table: "FamilyTree");

            migrationBuilder.DropTable(
                name: "ApplicationUserFamilyTree");

            migrationBuilder.AddColumn<long>(
                name: "FamilyTreeId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

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
    }
}
