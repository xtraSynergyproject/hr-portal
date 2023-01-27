using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210604_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "AllowedTagCategories",
                schema: "log",
                table: "TemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedTagCategories",
                schema: "public",
                table: "Template",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedTagCategories",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "AllowedTagCategories",
                schema: "public",
                table: "Template");
        }
    }
}
