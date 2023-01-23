using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210701_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "public",
                table: "Page",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PageDetails",
                schema: "public",
                table: "Page",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "MenuGroupDetails",
                schema: "log",
                table: "MenuGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "MenuGroupIconFileId",
                schema: "log",
                table: "MenuGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "MenuGroupDetails",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "MenuGroupIconFileId",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "PageDetails",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "MenuGroupDetails",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.DropColumn(
                name: "MenuGroupIconFileId",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.DropColumn(
                name: "MenuGroupDetails",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.DropColumn(
                name: "MenuGroupIconFileId",
                schema: "public",
                table: "MenuGroup");
        }
    }
}
