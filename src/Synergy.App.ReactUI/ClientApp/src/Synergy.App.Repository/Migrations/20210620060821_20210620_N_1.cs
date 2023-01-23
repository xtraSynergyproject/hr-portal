using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210620_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HideOwner",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideOwner",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideOwner",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideSLA",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideOwner",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideSLA",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideOwner",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideOwner",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HideOwner",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideOwner",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideOwner",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideSLA",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideOwner",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideSLA",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideOwner",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideOwner",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
