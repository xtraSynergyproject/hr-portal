using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220318_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "public",
                table: "Page",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "log",
                table: "MenuGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "public",
                table: "MenuGroup");
        }
    }
}
