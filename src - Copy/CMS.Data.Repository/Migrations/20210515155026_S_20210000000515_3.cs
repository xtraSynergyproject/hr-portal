using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210000000515_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrintJson",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrintJson",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnablePrintButton",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrintButtonText",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnablePrintButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrintButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocTaskTemplateIds",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePrintButton",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrintButtonText",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string[]>(
                name: "AdhocTaskTemplateIds",
                schema: "public",
                table: "ServiceTemplate",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePrintButton",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrintButtonText",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnablePrintButton",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrintButtonText",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnablePrintButton",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrintButtonText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrintJson",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "PrintJson",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "EnablePrintButton",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "PrintButtonText",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnablePrintButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PrintButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocTaskTemplateIds",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnablePrintButton",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "PrintButtonText",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "AdhocTaskTemplateIds",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EnablePrintButton",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "PrintButtonText",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EnablePrintButton",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "PrintButtonText",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnablePrintButton",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "PrintButtonText",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
