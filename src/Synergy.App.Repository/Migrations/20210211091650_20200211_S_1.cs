using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20200211_S_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AuthorizationNotRequired",
                schema: "public",
                table: "Page",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DontShowMenuInThisPage",
                schema: "public",
                table: "Page",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowOutsideMenuGroup",
                schema: "public",
                table: "Page",
                type: "boolean",
                nullable: false,
                defaultValue: false);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizationNotRequired",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "DontShowMenuInThisPage",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "ShowOutsideMenuGroup",
                schema: "public",
                table: "Page");

          
        }
    }
}
