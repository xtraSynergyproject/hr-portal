using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220309_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hindi",
                schema: "public",
                table: "ResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "HindiHelperText",
                schema: "public",
                table: "ResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "HindiTooltip",
                schema: "public",
                table: "ResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hindi",
                schema: "public",
                table: "ResourceLanguage");

            migrationBuilder.DropColumn(
                name: "HindiHelperText",
                schema: "public",
                table: "ResourceLanguage");

            migrationBuilder.DropColumn(
                name: "HindiTooltip",
                schema: "public",
                table: "ResourceLanguage");
        }
    }
}
