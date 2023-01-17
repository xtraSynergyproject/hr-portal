using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210527_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDashboard",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDashboard",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDashboard",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsDashboard",
                schema: "public",
                table: "CustomTemplate");
        }
    }
}
