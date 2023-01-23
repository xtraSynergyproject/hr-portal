using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210921_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomTemplateType",
                schema: "log",
                table: "CustomTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomTemplateType",
                schema: "public",
                table: "CustomTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomTemplateType",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "CustomTemplateType",
                schema: "public",
                table: "CustomTemplate");
        }
    }
}
