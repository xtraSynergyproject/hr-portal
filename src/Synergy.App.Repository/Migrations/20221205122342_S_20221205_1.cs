using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221205_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "French",
                schema: "public",
                table: "ResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Spanish",
                schema: "public",
                table: "ResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "French",
                schema: "log",
                table: "NoteResourceLanguageLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Spanish",
                schema: "log",
                table: "NoteResourceLanguageLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "French",
                schema: "public",
                table: "NoteResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Spanish",
                schema: "public",
                table: "NoteResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameFrench",
                schema: "log",
                table: "LOVLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameSpanish",
                schema: "log",
                table: "LOVLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameFrench",
                schema: "public",
                table: "LOV",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameSpanish",
                schema: "public",
                table: "LOV",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "French",
                schema: "log",
                table: "FormResourceLanguageLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Spanish",
                schema: "log",
                table: "FormResourceLanguageLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "French",
                schema: "public",
                table: "FormResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Spanish",
                schema: "public",
                table: "FormResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "French",
                schema: "public",
                table: "ResourceLanguage");

            migrationBuilder.DropColumn(
                name: "Spanish",
                schema: "public",
                table: "ResourceLanguage");

            migrationBuilder.DropColumn(
                name: "French",
                schema: "log",
                table: "NoteResourceLanguageLog");

            migrationBuilder.DropColumn(
                name: "Spanish",
                schema: "log",
                table: "NoteResourceLanguageLog");

            migrationBuilder.DropColumn(
                name: "French",
                schema: "public",
                table: "NoteResourceLanguage");

            migrationBuilder.DropColumn(
                name: "Spanish",
                schema: "public",
                table: "NoteResourceLanguage");

            migrationBuilder.DropColumn(
                name: "NameFrench",
                schema: "log",
                table: "LOVLog");

            migrationBuilder.DropColumn(
                name: "NameSpanish",
                schema: "log",
                table: "LOVLog");

            migrationBuilder.DropColumn(
                name: "NameFrench",
                schema: "public",
                table: "LOV");

            migrationBuilder.DropColumn(
                name: "NameSpanish",
                schema: "public",
                table: "LOV");

            migrationBuilder.DropColumn(
                name: "French",
                schema: "log",
                table: "FormResourceLanguageLog");

            migrationBuilder.DropColumn(
                name: "Spanish",
                schema: "log",
                table: "FormResourceLanguageLog");

            migrationBuilder.DropColumn(
                name: "French",
                schema: "public",
                table: "FormResourceLanguage");

            migrationBuilder.DropColumn(
                name: "Spanish",
                schema: "public",
                table: "FormResourceLanguage");
        }
    }
}
