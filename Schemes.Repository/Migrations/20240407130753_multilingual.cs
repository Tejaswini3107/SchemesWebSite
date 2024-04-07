using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schemes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class multilingual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MultilingualSchemesData",
                columns: table => new
                {
                    MultilingualSchemesDataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchemesDetailID = table.Column<int>(type: "int", nullable: false),
                    LangCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvaliableFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOftheScheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityCriteria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentsRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplyAndLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultilingualSchemesData", x => x.MultilingualSchemesDataID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultilingualSchemesData");
        }
    }
}
