using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20211202_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundFileId",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BannerFileId",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateColor",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundFileId",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BannerFileId",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateColor",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundFileId",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "BannerFileId",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "TemplateColor",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "BackgroundFileId",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "BannerFileId",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "TemplateColor",
                schema: "public",
                table: "CustomTemplate");
        }
    }
}
