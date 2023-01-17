using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210927_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormType",
                schema: "log",
                table: "TaskTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormType",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormType",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormType",
                schema: "public",
                table: "ServiceTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormType",
                schema: "log",
                table: "NoteTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormType",
                schema: "public",
                table: "NoteTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormType",
                schema: "log",
                table: "FormTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormType",
                schema: "public",
                table: "FormTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormType",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "FormType",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "FormType",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "FormType",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "FormType",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "FormType",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "FormType",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropColumn(
                name: "FormType",
                schema: "public",
                table: "FormTemplate");
        }
    }
}
