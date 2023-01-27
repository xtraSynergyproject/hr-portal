using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210902_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceTemplateType",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceTemplateType",
                schema: "public",
                table: "ServiceTemplate",
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
                name: "ServiceTemplateType",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "ServiceTemplateType",
                schema: "public",
                table: "ServiceTemplate");
 

            migrationBuilder.DropColumn(
                name: "NoteTemplateType",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
