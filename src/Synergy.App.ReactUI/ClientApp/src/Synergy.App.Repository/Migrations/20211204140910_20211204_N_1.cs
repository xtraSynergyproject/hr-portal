using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20211204_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "AllowedPortalIds",
                schema: "log",
                table: "UserGroupLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedPortalIds",
                schema: "public",
                table: "UserGroup",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedPortalIds",
                schema: "log",
                table: "TeamLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AllowedPortalIds",
                schema: "public",
                table: "Team",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedPortalIds",
                schema: "log",
                table: "UserGroupLog");

            migrationBuilder.DropColumn(
                name: "AllowedPortalIds",
                schema: "public",
                table: "UserGroup");

            migrationBuilder.DropColumn(
                name: "AllowedPortalIds",
                schema: "log",
                table: "TeamLog");

            migrationBuilder.DropColumn(
                name: "AllowedPortalIds",
                schema: "public",
                table: "Team");
        }
    }
}
