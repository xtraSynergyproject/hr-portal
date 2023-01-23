using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210703_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomTemplateLoadingType",
                schema: "log",
                table: "CustomTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JavascriptName",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "JavascriptParam",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "CustomTemplateLoadingType",
                schema: "public",
                table: "CustomTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JavascriptName",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "JavascriptParam",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomTemplateLoadingType",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "JavascriptName",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "JavascriptParam",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "CustomTemplateLoadingType",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "JavascriptName",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "JavascriptParam",
                schema: "public",
                table: "CustomTemplate");
        }
    }
}
