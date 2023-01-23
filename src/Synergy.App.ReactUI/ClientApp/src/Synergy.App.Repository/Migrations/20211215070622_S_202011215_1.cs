using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_202011215_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
