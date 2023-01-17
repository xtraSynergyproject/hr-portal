using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210524_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Users",
                schema: "log",
                table: "UserSetLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "Users",
                schema: "public",
                table: "UserSet",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Users",
                schema: "log",
                table: "UserSetLog");

            migrationBuilder.DropColumn(
                name: "Users",
                schema: "public",
                table: "UserSet");
        }
    }
}
