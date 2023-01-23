using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20200212_S_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowMenuWhenAuthorized",
                schema: "public",
                table: "Page",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowMenuWhenNotAuthorized",
                schema: "public",
                table: "Page",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowMenuWhenAuthorized",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "ShowMenuWhenNotAuthorized",
                schema: "public",
                table: "Page");
        }
    }
}
