using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210728_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "IsNumberNotMandatory",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberGenerationType",
                schema: "log",
                table: "TaskTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNumberNotMandatory",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberGenerationType",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNumberNotMandatory",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberGenerationType",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNumberNotMandatory",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberGenerationType",
                schema: "public",
                table: "ServiceTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNumberNotMandatory",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberGenerationType",
                schema: "log",
                table: "NoteTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNumberNotMandatory",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberGenerationType",
                schema: "public",
                table: "NoteTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNumberNotMandatory",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "NumberGenerationType",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsNumberNotMandatory",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "NumberGenerationType",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsNumberNotMandatory",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "NumberGenerationType",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsNumberNotMandatory",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "NumberGenerationType",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IsNumberNotMandatory",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "NumberGenerationType",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsNumberNotMandatory",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "NumberGenerationType",
                schema: "public",
                table: "NoteTemplate");

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
    }
}
