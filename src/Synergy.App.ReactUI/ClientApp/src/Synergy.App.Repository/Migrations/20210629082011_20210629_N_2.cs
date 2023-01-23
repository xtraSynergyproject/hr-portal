using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210629_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "LegalEntityIds",
                schema: "log",
                table: "UserLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "LegalEntityIds",
                schema: "public",
                table: "User",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableLegalEntity",
                schema: "log",
                table: "PortalLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableLegalEntity",
                schema: "public",
                table: "Portal",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalEntityIds",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityIds",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EnableLegalEntity",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "EnableLegalEntity",
                schema: "public",
                table: "Portal");
        }
    }
}
