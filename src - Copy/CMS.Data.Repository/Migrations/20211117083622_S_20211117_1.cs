using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20211117_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisableReopen",
                schema: "log",
                table: "NtsServiceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableReopen",
                schema: "public",
                table: "NtsService",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisableReopen",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "DisableReopen",
                schema: "public",
                table: "NtsService");
        }
    }
}
