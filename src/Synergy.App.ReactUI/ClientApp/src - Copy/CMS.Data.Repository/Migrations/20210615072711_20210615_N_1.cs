using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210615_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LetterFooterId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LetterHeaderId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LetterFooterId",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LetterHeaderId",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LetterFooterId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LetterHeaderId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LetterFooterId",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LetterHeaderId",
                schema: "public",
                table: "Company");
        }
    }
}
