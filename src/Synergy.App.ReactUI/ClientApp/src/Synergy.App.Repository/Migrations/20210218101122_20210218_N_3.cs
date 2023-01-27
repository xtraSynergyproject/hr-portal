using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210218_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfArrival",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextOfApplicant",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextOfApplicantEmail",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextOfApplicantPhoneNo",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextOfApplicantRelationship",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherNextOfApplicant",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherNextOfApplicantEmail",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherNextOfApplicantPhoneNo",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherNextOfApplicantRelationship",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReportingToId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Sourcing",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "WitnessDate1",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WitnessDate2",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WitnessDesignation1",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "WitnessDesignation2",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "WitnessGAEC1",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "WitnessGAEC2",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "WitnessName1",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "WitnessName2",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfArrival",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "NextOfApplicant",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "NextOfApplicantEmail",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "NextOfApplicantPhoneNo",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "NextOfApplicantRelationship",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherNextOfApplicant",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherNextOfApplicantEmail",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherNextOfApplicantPhoneNo",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherNextOfApplicantRelationship",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ReportingToId",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "Sourcing",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "WitnessDate1",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "WitnessDate2",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "WitnessDesignation1",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "WitnessDesignation2",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "WitnessGAEC1",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "WitnessGAEC2",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "WitnessName1",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "WitnessName2",
                schema: "rec",
                table: "Application");
        }
    }
}
