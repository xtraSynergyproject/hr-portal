using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220617_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameArabic",
                schema: "log",
                table: "LOVLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameHindi",
                schema: "log",
                table: "LOVLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameArabic",
                schema: "public",
                table: "LOV",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameHindi",
                schema: "public",
                table: "LOV",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "NameArabic",
                schema: "log",
                table: "LOVLog");

            migrationBuilder.DropColumn(
                name: "NameHindi",
                schema: "log",
                table: "LOVLog");

            migrationBuilder.DropColumn(
                name: "NameArabic",
                schema: "public",
                table: "LOV");

            migrationBuilder.DropColumn(
                name: "NameHindi",
                schema: "public",
                table: "LOV");
        }
    }
}
