using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20211204_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
