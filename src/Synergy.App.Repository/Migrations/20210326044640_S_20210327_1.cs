using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210327_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SLA",
                schema: "public",
                table: "NtsTask",
                newName: "TaskSLA");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SLA",
                schema: "public",
                table: "TaskTemplate",
                type: "interval",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SLA",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.RenameColumn(
                name: "TaskSLA",
                schema: "public",
                table: "NtsTask",
                newName: "SLA");
        }
    }
}
