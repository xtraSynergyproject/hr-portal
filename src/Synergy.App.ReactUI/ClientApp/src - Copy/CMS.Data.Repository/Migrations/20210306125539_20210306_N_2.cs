using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210306_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode1",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode2",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode3",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode4",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode5",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue1",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue2",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue3",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue4",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue5",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue1",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue2",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue3",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue4",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue5",
                schema: "public",
                table: "RecTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentVersionNo",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentCode1",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode2",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode3",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode4",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentCode5",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue1",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue2",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue3",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue4",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "AttachmentValue5",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue1",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue2",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue3",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue4",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "DatePickerValue5",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "ParentVersionNo",
                schema: "public",
                table: "RecTaskVersion");
        }
    }
}
