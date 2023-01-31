using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210825_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExpandHelpPanel",
                schema: "public",
                table: "Page",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExpandHelpPanel",
                schema: "log",
                table: "MenuGroupLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExpandHelpPanel",
                schema: "public",
                table: "MenuGroup",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpandHelpPanel",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "ExpandHelpPanel",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.DropColumn(
                name: "ExpandHelpPanel",
                schema: "public",
                table: "MenuGroup");
        }
    }
}
