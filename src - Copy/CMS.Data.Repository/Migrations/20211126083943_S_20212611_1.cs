using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20212611_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "AllowedLanguageIds",
                schema: "log",
                table: "PortalLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedLanguageIds",
                schema: "public",
                table: "Portal",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiValueColumn",
                schema: "log",
                table: "ColumnMetadataLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiValueColumn",
                schema: "public",
                table: "ColumnMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedLanguageIds",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "AllowedLanguageIds",
                schema: "public",
                table: "Portal");

            migrationBuilder.DropColumn(
                name: "IsMultiValueColumn",
                schema: "log",
                table: "ColumnMetadataLog");

            migrationBuilder.DropColumn(
                name: "IsMultiValueColumn",
                schema: "public",
                table: "ColumnMetadata");
        }
    }
}
