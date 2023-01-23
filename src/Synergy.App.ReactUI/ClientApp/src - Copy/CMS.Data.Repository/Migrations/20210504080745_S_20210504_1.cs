using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210504_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HideBanner",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideToolbar",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideBanner",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideToolbar",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideBanner",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideToolbar",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideBanner",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideToolbar",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideBanner",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideToolbar",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideBanner",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideToolbar",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HideBanner",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideToolbar",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideBanner",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideToolbar",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideBanner",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideToolbar",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideBanner",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideToolbar",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideBanner",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideToolbar",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "HideBanner",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "HideToolbar",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
