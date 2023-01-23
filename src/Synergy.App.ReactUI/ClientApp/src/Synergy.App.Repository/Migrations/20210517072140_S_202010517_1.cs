using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_202010517_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableEmail",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableEmail",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableEmail",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableEmail",
                schema: "public",
                table: "TaskTemplate");
        }
    }
}
