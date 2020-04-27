using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApp.Data.Migrations
{
    public partial class newrealtionships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookTypes_BookTypeId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookTypeId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookTypeId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "BookTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookTypes_BookId",
                table: "BookTypes",
                column: "BookId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookTypes_Books_BookId",
                table: "BookTypes",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookTypes_Books_BookId",
                table: "BookTypes");

            migrationBuilder.DropIndex(
                name: "IX_BookTypes_BookId",
                table: "BookTypes");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BookTypes");

            migrationBuilder.AddColumn<int>(
                name: "BookTypeId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookTypeId",
                table: "Books",
                column: "BookTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookTypes_BookTypeId",
                table: "Books",
                column: "BookTypeId",
                principalTable: "BookTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
