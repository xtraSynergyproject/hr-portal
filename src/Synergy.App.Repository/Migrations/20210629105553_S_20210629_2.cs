using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210629_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemlateColor",
                schema: "log",
                table: "TaskTemplateLog",
                newName: "TemplateColor");

            migrationBuilder.RenameColumn(
                name: "TemlateColor",
                schema: "public",
                table: "TaskTemplate",
                newName: "TemplateColor");

            migrationBuilder.RenameColumn(
                name: "TemlateColor",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "TemplateColor");

            migrationBuilder.RenameColumn(
                name: "TemlateColor",
                schema: "public",
                table: "ServiceTemplate",
                newName: "TemplateColor");

            migrationBuilder.RenameColumn(
                name: "TemlateColor",
                schema: "log",
                table: "NoteTemplateLog",
                newName: "TemplateColor");

            migrationBuilder.RenameColumn(
                name: "TemlateColor",
                schema: "public",
                table: "NoteTemplate",
                newName: "TemplateColor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemplateColor",
                schema: "log",
                table: "TaskTemplateLog",
                newName: "TemlateColor");

            migrationBuilder.RenameColumn(
                name: "TemplateColor",
                schema: "public",
                table: "TaskTemplate",
                newName: "TemlateColor");

            migrationBuilder.RenameColumn(
                name: "TemplateColor",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "TemlateColor");

            migrationBuilder.RenameColumn(
                name: "TemplateColor",
                schema: "public",
                table: "ServiceTemplate",
                newName: "TemlateColor");

            migrationBuilder.RenameColumn(
                name: "TemplateColor",
                schema: "log",
                table: "NoteTemplateLog",
                newName: "TemlateColor");

            migrationBuilder.RenameColumn(
                name: "TemplateColor",
                schema: "public",
                table: "NoteTemplate",
                newName: "TemlateColor");
        }
    }
}
