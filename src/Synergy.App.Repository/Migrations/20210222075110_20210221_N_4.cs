using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210221_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AnnualLeave",
                schema: "rec",
                table: "Application",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractStartDate",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrainee",
                schema: "rec",
                table: "Application",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobNo",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningNotLaterThan",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ServiceCompletion",
                schema: "rec",
                table: "Application",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TravelOriginAndDestination",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "VehicleTransport",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualLeave",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ContractStartDate",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "IsTrainee",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "JobNo",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "JoiningNotLaterThan",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ServiceCompletion",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "TravelOriginAndDestination",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "VehicleTransport",
                schema: "rec",
                table: "Application");
        }
    }
}
