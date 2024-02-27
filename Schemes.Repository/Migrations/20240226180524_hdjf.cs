using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schemes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class hdjf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "CustomerLogin");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "CustomerLogin",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "CustomerLogin",
                newName: "PasswordSalt");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "CustomerLogin",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
