using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220325_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "AllowedPortalIds",
                schema: "log",
                table: "TemplateCategoryLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedPortalIds",
                schema: "public",
                table: "TemplateCategory",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedPortalIds",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropColumn(
                name: "AllowedPortalIds",
                schema: "public",
                table: "TemplateCategory");
        }
    }
}
