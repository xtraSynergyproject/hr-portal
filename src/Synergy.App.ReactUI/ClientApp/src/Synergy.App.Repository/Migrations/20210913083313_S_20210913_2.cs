using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210913_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NtsType",
                schema: "log",
                table: "TemplateLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NtsType",
                schema: "public",
                table: "Template",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NtsType",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "NtsType",
                schema: "public",
                table: "Template");
        }
    }
}
