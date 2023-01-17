using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210607_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemplateCategoryType",
                schema: "log",
                table: "TemplateCategoryLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TemplateCategoryType",
                schema: "public",
                table: "TemplateCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateCategoryType",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropColumn(
                name: "TemplateCategoryType",
                schema: "public",
                table: "TemplateCategory");
        }
    }
}
