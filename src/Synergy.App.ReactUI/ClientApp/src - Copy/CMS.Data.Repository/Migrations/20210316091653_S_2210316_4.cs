using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210316_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SLA",
                schema: "public",
                table: "NtsService");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ActualSLA",
                schema: "public",
                table: "NtsTask",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualSLA",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.AddColumn<string>(
                name: "SLA",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
