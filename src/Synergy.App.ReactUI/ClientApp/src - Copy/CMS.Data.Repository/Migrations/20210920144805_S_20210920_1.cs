using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210920_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailCopyTemplateCode",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "EmailCopyTemplateCode",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "EmailCopyTemplateCode",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "EmailCopyTemplateCode",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "EmailCopyTemplateCode",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "EmailCopyTemplateCode",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailCopyTemplateCode",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "EmailCopyTemplateCode",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EmailCopyTemplateCode",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EmailCopyTemplateCode",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EmailCopyTemplateCode",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "EmailCopyTemplateCode",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
