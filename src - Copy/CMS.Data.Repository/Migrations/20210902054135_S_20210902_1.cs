using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210902_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewType",
                schema: "log",
                table: "TemplateLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewType",
                schema: "public",
                table: "Template",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocNoteTemplateIds",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocServiceTemplateIds",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocNoteTemplateIds",
                schema: "public",
                table: "ServiceTemplate",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocServiceTemplateIds",
                schema: "public",
                table: "ServiceTemplate",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocNoteTemplateIds",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocServiceTemplateIds",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocTaskTemplateIds",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocNoteTemplateIds",
                schema: "public",
                table: "NoteTemplate",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocServiceTemplateIds",
                schema: "public",
                table: "NoteTemplate",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocTaskTemplateIds",
                schema: "public",
                table: "NoteTemplate",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewType",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "ViewType",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "AdhocNoteTemplateIds",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "AdhocServiceTemplateIds",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "AdhocNoteTemplateIds",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocServiceTemplateIds",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocNoteTemplateIds",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "AdhocServiceTemplateIds",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "AdhocTaskTemplateIds",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "AdhocNoteTemplateIds",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocServiceTemplateIds",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocTaskTemplateIds",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
