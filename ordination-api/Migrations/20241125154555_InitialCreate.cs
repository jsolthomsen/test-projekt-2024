using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ordination_api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordinationer_Laegemiddler_LaegemiddelId",
                table: "Ordinationer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Laegemiddler",
                table: "Laegemiddler");

            migrationBuilder.RenameTable(
                name: "Laegemiddler",
                newName: "Laegemidler");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Laegemidler",
                table: "Laegemidler",
                column: "LaegemiddelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordinationer_Laegemidler_LaegemiddelId",
                table: "Ordinationer",
                column: "LaegemiddelId",
                principalTable: "Laegemidler",
                principalColumn: "LaegemiddelId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordinationer_Laegemidler_LaegemiddelId",
                table: "Ordinationer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Laegemidler",
                table: "Laegemidler");

            migrationBuilder.RenameTable(
                name: "Laegemidler",
                newName: "Laegemiddler");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Laegemiddler",
                table: "Laegemiddler",
                column: "LaegemiddelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordinationer_Laegemiddler_LaegemiddelId",
                table: "Ordinationer",
                column: "LaegemiddelId",
                principalTable: "Laegemiddler",
                principalColumn: "LaegemiddelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
