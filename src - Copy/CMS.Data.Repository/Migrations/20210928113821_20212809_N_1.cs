using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20212809_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReopened",
                schema: "log",
                table: "NtsServiceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReopened",
                schema: "public",
                table: "NtsService",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReopened",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "IsReopened",
                schema: "public",
                table: "NtsService");
        }
    }
}
