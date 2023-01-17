using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220618_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableLanguageValidation",
                schema: "log",
                table: "ColumnMetadataLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableLocalization",
                schema: "log",
                table: "ColumnMetadataLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableLanguageValidation",
                schema: "public",
                table: "ColumnMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableLocalization",
                schema: "public",
                table: "ColumnMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableLanguageValidation",
                schema: "log",
                table: "ColumnMetadataLog");

            migrationBuilder.DropColumn(
                name: "EnableLocalization",
                schema: "log",
                table: "ColumnMetadataLog");

            migrationBuilder.DropColumn(
                name: "EnableLanguageValidation",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "EnableLocalization",
                schema: "public",
                table: "ColumnMetadata");
        }
    }
}
