using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210220_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConstructionPeriodFrom",
                schema: "rec",
                table: "CandidateProject",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConstructionPeriodTo",
                schema: "rec",
                table: "CandidateProject",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConstructionPeriodFrom",
                schema: "rec",
                table: "ApplicationProject",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConstructionPeriodTo",
                schema: "rec",
                table: "ApplicationProject",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstructionPeriodFrom",
                schema: "rec",
                table: "CandidateProject");

            migrationBuilder.DropColumn(
                name: "ConstructionPeriodTo",
                schema: "rec",
                table: "CandidateProject");

            migrationBuilder.DropColumn(
                name: "ConstructionPeriodFrom",
                schema: "rec",
                table: "ApplicationProject");

            migrationBuilder.DropColumn(
                name: "ConstructionPeriodTo",
                schema: "rec",
                table: "ApplicationProject");
        }
    }
}
