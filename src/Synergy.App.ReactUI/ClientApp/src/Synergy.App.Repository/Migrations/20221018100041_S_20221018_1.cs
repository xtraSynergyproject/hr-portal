using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221018_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "log",
                table: "NtsTaskSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "public",
                table: "NtsTaskSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "log",
                table: "NtsServiceSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "public",
                table: "NtsServiceSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "log",
                table: "NtsNoteSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "public",
                table: "NtsNoteSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "log",
                table: "NtsTaskSequenceLog");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "public",
                table: "NtsTaskSequence");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "log",
                table: "NtsServiceSequenceLog");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "public",
                table: "NtsServiceSequence");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "log",
                table: "NtsNoteSequenceLog");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "public",
                table: "NtsNoteSequence");
        }
    }
}
