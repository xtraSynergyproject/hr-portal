using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20211221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActionButtonPosition",
                schema: "log",
                table: "TaskTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionButtonPosition",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionButtonPosition",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionButtonPosition",
                schema: "public",
                table: "ServiceTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionButtonPosition",
                schema: "log",
                table: "NoteTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionButtonPosition",
                schema: "public",
                table: "NoteTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionButtonPosition",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "ActionButtonPosition",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ActionButtonPosition",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "ActionButtonPosition",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "ActionButtonPosition",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "ActionButtonPosition",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
