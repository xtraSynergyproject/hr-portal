using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210902_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoteTemplateType",
                schema: "log",
                table: "NoteTemplateLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoteTemplateType",
                schema: "public",
                table: "NoteTemplate",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteTemplateType",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "NoteTemplateType",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
