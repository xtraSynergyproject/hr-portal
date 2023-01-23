using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210914_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableAccordianMenu",
                schema: "log",
                table: "PortalLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableAccordianMenu",
                schema: "public",
                table: "Portal",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableAccordianMenu",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "EnableAccordianMenu",
                schema: "public",
                table: "Portal");
        }
    }
}
