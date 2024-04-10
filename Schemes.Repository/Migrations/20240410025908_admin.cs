using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schemes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AdminLogin");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "AdminLogin",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "AdminLogin",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AdminLogin",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "AdminLogin",
                newName: "PasswordSalt");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AdminLogin",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
