using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210504_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowOldStartDate",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowOldStartDate",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowOldStartDate",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowOldStartDate",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowOldStartDate",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowOldStartDate",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowOldStartDate",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "AllowOldStartDate",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AllowOldStartDate",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "AllowOldStartDate",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "AllowOldStartDate",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "AllowOldStartDate",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
