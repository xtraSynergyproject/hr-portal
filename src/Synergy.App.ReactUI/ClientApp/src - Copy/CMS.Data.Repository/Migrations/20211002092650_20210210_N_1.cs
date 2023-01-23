using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210210_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableServiceComplete",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableServiceComplete",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableServiceComplete",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "EnableServiceComplete",
                schema: "public",
                table: "StepTaskComponent");
        }
    }
}
