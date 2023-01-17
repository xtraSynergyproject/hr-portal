using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210728_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNumberNotMandatory",
                schema: "log",
                table: "TemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberGenerationType",
                schema: "log",
                table: "TemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNumberNotMandatory",
                schema: "public",
                table: "Template",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberGenerationType",
                schema: "public",
                table: "Template",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNumberNotMandatory",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "NumberGenerationType",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "IsNumberNotMandatory",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "NumberGenerationType",
                schema: "public",
                table: "Template");
        }
    }
}
