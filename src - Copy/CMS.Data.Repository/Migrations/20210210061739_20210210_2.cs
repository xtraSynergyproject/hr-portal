using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210210_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PassingYear",
                schema: "rec",
                table: "CandidateEducational",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PassingYear",
                schema: "rec",
                table: "ApplicationEducational",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassingYear",
                schema: "rec",
                table: "CandidateEducational");

            migrationBuilder.DropColumn(
                name: "PassingYear",
                schema: "rec",
                table: "ApplicationEducational");
        }
    }
}
