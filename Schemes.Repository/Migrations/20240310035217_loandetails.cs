using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schemes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class loandetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "OTPDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LoanInterestDetails",
                columns: table => new
                {
                    LoanInterestDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinLoanAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxloanAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinInterestRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxInterestRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanInterestDetails", x => x.LoanInterestDetailID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanInterestDetails");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "OTPDetails");
        }
    }
}
