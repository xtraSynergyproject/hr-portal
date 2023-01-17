using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _202103_T_11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode10",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode6",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode7",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode8",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode9",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue10",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue6",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue7",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue8",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue9",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue10",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue6",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue7",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue8",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue9",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode10",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode6",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode7",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode8",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode9",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue10",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue6",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue7",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue8",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue9",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue10",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue6",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue7",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue8",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue9",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentCode10",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode6",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode7",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode8",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode9",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue10",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue6",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue7",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue8",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue9",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue10",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue6",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue7",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue8",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue9",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode10",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentCode6",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentCode7",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentCode8",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentCode9",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue10",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue6",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue7",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue8",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue9",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue10",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue6",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue7",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue8",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue9",
                schema: "public",
                table: "RecTask");
        }
    }
}
