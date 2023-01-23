using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _202102173 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GaecNo",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningDate",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfferSignedBy",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GaecNo",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OfferSignedBy",
                schema: "rec",
                table: "Application");
        }
    }
}
