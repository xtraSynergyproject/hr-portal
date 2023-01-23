using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210302_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowUnderMenuGroup",
                schema: "public",
                table: "Page");

            migrationBuilder.AddColumn<string>(
                name: "IconColor",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IconCss",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconColor",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.DropColumn(
                name: "IconCss",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.AddColumn<bool>(
                name: "ShowUnderMenuGroup",
                schema: "public",
                table: "Page",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
