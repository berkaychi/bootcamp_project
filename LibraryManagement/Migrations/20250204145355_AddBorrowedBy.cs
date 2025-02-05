using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_RentedByUserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_RentedByUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RentedByUserId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "BorrowedBy",
                table: "Books",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowedBy",
                table: "Books");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RentedByUserId",
                table: "Books",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_RentedByUserId",
                table: "Books",
                column: "RentedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_RentedByUserId",
                table: "Books",
                column: "RentedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
