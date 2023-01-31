using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220823_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                schema: "log",
                table: "NtsCategoryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                schema: "public",
                table: "NtsCategory",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedCategories",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedTemplates",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedCategories",
                schema: "public",
                table: "CustomTemplate",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedTemplates",
                schema: "public",
                table: "CustomTemplate",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                schema: "log",
                table: "NtsCategoryLog");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                schema: "public",
                table: "NtsCategory");

            migrationBuilder.DropColumn(
                name: "AllowedCategories",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "AllowedTemplates",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "AllowedCategories",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "AllowedTemplates",
                schema: "public",
                table: "CustomTemplate");
        }
    }
}
