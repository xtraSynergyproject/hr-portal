using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20211204_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "LicensedPortslIds",
                schema: "log",
                table: "CompanyLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "LicensedPortslIds",
                schema: "public",
                table: "Company",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicensedPortslIds",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LicensedPortslIds",
                schema: "public",
                table: "Company");
        }
    }
}
