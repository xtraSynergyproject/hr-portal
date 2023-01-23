using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210210_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassingYear",
                schema: "rec",
                table: "CandidateEducational");

            migrationBuilder.DropColumn(
                name: "PassingYear",
                schema: "rec",
                table: "ApplicationEducational");

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "SignatureDate",
                schema: "rec",
                table: "CandidateProfile",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "SignatureDate",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "SignatureDate",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "Signature",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "SignatureDate",
                schema: "rec",
                table: "Application");

            migrationBuilder.AddColumn<string>(
                name: "PassingYear",
                schema: "rec",
                table: "CandidateEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PassingYear",
                schema: "rec",
                table: "ApplicationEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
