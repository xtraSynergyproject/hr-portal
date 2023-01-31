using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210526_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                schema: "public",
                table: "ProjectEmailSetup",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                schema: "log",
                table: "CompanyLog",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                schema: "public",
                table: "Company",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                schema: "public",
                table: "ProjectEmailSetup");

            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                schema: "public",
                table: "Company");
        }
    }
}
