using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210727_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkFlowTemplateId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "WorkFlowTemplateId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "DisablePermissionInheritance",
                schema: "log",
                table: "NtsNoteLog",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "log",
                table: "NtsNoteLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLockedDate",
                schema: "log",
                table: "NtsNoteLog",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LockStatus",
                schema: "log",
                table: "NtsNoteLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisablePermissionInheritance",
                schema: "public",
                table: "NtsNote",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "public",
                table: "NtsNote",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLockedDate",
                schema: "public",
                table: "NtsNote",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LockStatus",
                schema: "public",
                table: "NtsNote",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InheritedFrom",
                schema: "public",
                table: "DocumentPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateLog_WorkFlowTemplateId",
                schema: "log",
                table: "TemplateLog",
                column: "WorkFlowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_WorkFlowTemplateId",
                schema: "public",
                table: "Template",
                column: "WorkFlowTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_Template_WorkFlowTemplateId",
                schema: "public",
                table: "Template",
                column: "WorkFlowTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateLog_Template_WorkFlowTemplateId",
                schema: "log",
                table: "TemplateLog",
                column: "WorkFlowTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_Template_WorkFlowTemplateId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateLog_Template_WorkFlowTemplateId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TemplateLog_WorkFlowTemplateId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_Template_WorkFlowTemplateId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "WorkFlowTemplateId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "WorkFlowTemplateId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "DisablePermissionInheritance",
                schema: "log",
                table: "NtsNoteLog");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "log",
                table: "NtsNoteLog");

            migrationBuilder.DropColumn(
                name: "LastLockedDate",
                schema: "log",
                table: "NtsNoteLog");

            migrationBuilder.DropColumn(
                name: "LockStatus",
                schema: "log",
                table: "NtsNoteLog");

            migrationBuilder.DropColumn(
                name: "DisablePermissionInheritance",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "LastLockedDate",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "LockStatus",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "InheritedFrom",
                schema: "public",
                table: "DocumentPermission");
        }
    }
}
