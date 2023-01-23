using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220617_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QualificationType",
                schema: "rec",
                table: "CandidateEducational",
                newName: "QualificationTypeId");

            migrationBuilder.RenameColumn(
                name: "EducationType",
                schema: "rec",
                table: "CandidateEducational",
                newName: "EducationTypeId");

            migrationBuilder.AddColumn<string>(
                name: "LocalizedColumnId",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LocalizedColumnId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "OpenSameTaskOnServiceSubmit",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableIntroPage",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePreviewPage",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IntroPageAction",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IntroPageArea",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IntroPageController",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IntroPageParams",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableIntroPage",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePreviewPage",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IntroPageAction",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IntroPageArea",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IntroPageController",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IntroPageParams",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LocalizedColumnId",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LocalizedColumnId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<string>(
                name: "AgencyId",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LocalizedColumnId",
                schema: "log",
                table: "FormTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LocalizedColumnId",
                schema: "public",
                table: "FormTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "TaskTemplateLog",
                column: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplate_LocalizedColumnId",
                schema: "public",
                table: "TaskTemplate",
                column: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate",
                column: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "NoteTemplateLog",
                column: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTemplate_LocalizedColumnId",
                schema: "public",
                table: "NoteTemplate",
                column: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FormTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "FormTemplateLog",
                column: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FormTemplate_LocalizedColumnId",
                schema: "public",
                table: "FormTemplate",
                column: "LocalizedColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "FormTemplate",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "FormTemplateLog",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "NoteTemplate",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "NoteTemplateLog",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "TaskTemplate",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "TaskTemplateLog",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FormTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplate_LocalizedColumnId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropIndex(
                name: "IX_NoteTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_NoteTemplate_LocalizedColumnId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FormTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_FormTemplate_LocalizedColumnId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropColumn(
                name: "LocalizedColumnId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "LocalizedColumnId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "OpenSameTaskOnServiceSubmit",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "EnableIntroPage",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnablePreviewPage",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "IntroPageAction",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "IntroPageArea",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "IntroPageController",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "IntroPageParams",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableIntroPage",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EnablePreviewPage",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IntroPageAction",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IntroPageArea",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IntroPageController",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IntroPageParams",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "LocalizedColumnId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "LocalizedColumnId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "LocalizedColumnId",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropColumn(
                name: "LocalizedColumnId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.RenameColumn(
                name: "QualificationTypeId",
                schema: "rec",
                table: "CandidateEducational",
                newName: "QualificationType");

            migrationBuilder.RenameColumn(
                name: "EducationTypeId",
                schema: "rec",
                table: "CandidateEducational",
                newName: "EducationType");

            migrationBuilder.AlterColumn<string[]>(
                name: "AgencyId",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
