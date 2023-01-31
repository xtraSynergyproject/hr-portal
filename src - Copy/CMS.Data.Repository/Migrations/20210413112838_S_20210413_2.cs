using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210413_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SLA",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.RenameColumn(
                name: "AllowSLAChange",
                schema: "public",
                table: "NoteTemplate",
                newName: "IsCancelReasonRequired");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CanceledDate",
                schema: "public",
                table: "NtsNote",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CloseComment",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                schema: "public",
                table: "NtsNote",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteActionId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<double>(
                name: "UserRating",
                schema: "public",
                table: "NtsNote",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelButtonCss",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CancelButtonText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableCancelButton",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NotificationSubject",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PriorityId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNote_NoteActionId",
                schema: "public",
                table: "NtsNote",
                column: "NoteActionId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTemplate_PriorityId",
                schema: "public",
                table: "NoteTemplate",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTemplate_LOV_PriorityId",
                schema: "public",
                table: "NoteTemplate",
                column: "PriorityId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNote_LOV_NoteActionId",
                schema: "public",
                table: "NtsNote",
                column: "NoteActionId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteTemplate_LOV_PriorityId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsNote_LOV_NoteActionId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropIndex(
                name: "IX_NtsNote_NoteActionId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropIndex(
                name: "IX_NoteTemplate_PriorityId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "CanceledDate",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "CloseComment",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "ClosedDate",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "NoteActionId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "UserRating",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "CancelButtonCss",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "CancelButtonText",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "EnableCancelButton",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "NotificationSubject",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.RenameColumn(
                name: "IsCancelReasonRequired",
                schema: "public",
                table: "NoteTemplate",
                newName: "AllowSLAChange");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SLA",
                schema: "public",
                table: "NoteTemplate",
                type: "interval",
                nullable: true);
        }
    }
}
