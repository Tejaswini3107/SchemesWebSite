using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schemes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedInserted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvaliableFor",
                table: "SchemesDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertedBy",
                table: "SchemesDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedDate",
                table: "SchemesDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SchemesDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SchemesDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertedBy",
                table: "OTPDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedDate",
                table: "OTPDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SentDetails",
                table: "OTPDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "OTPDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "OTPDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedDate",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertedBy",
                table: "CustomerLogin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedDate",
                table: "CustomerLogin",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CustomerLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CustomerLogin",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertedBy",
                table: "AdminLogin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedDate",
                table: "AdminLogin",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AdminLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AdminLogin",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertedBy",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedDate",
                table: "Admin",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Admin",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvaliableFor",
                table: "SchemesDetails");

            migrationBuilder.DropColumn(
                name: "InsertedBy",
                table: "SchemesDetails");

            migrationBuilder.DropColumn(
                name: "InsertedDate",
                table: "SchemesDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SchemesDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SchemesDetails");

            migrationBuilder.DropColumn(
                name: "InsertedBy",
                table: "OTPDetails");

            migrationBuilder.DropColumn(
                name: "InsertedDate",
                table: "OTPDetails");

            migrationBuilder.DropColumn(
                name: "SentDetails",
                table: "OTPDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "OTPDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "OTPDetails");

            migrationBuilder.DropColumn(
                name: "InsertedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "InsertedDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "InsertedBy",
                table: "CustomerLogin");

            migrationBuilder.DropColumn(
                name: "InsertedDate",
                table: "CustomerLogin");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CustomerLogin");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CustomerLogin");

            migrationBuilder.DropColumn(
                name: "InsertedBy",
                table: "AdminLogin");

            migrationBuilder.DropColumn(
                name: "InsertedDate",
                table: "AdminLogin");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AdminLogin");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AdminLogin");

            migrationBuilder.DropColumn(
                name: "InsertedBy",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "InsertedDate",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Admin");
        }
    }
}
