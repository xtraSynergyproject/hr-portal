using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210902_12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoteTemplateLog",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RecordId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LogVersionNo = table.Column<long>(type: "bigint", nullable: false),
                    IsLatest = table.Column<bool>(type: "boolean", nullable: false),
                    LogStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogStartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogEndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDatedLatest = table.Column<bool>(type: "boolean", nullable: false),
                    IsVersionLatest = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LegalEntityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DataAction = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableIndexPage = table.Column<bool>(type: "boolean", nullable: false),
                    EnableNoteNumberManual = table.Column<bool>(type: "boolean", nullable: false),
                    EnableSaveAsDraft = table.Column<bool>(type: "boolean", nullable: false),
                    SaveAsDraftText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SaveAsDraftCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableBackButton = table.Column<bool>(type: "boolean", nullable: false),
                    BackButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    BackButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableAttachment = table.Column<bool>(type: "boolean", nullable: false),
                    EnableComment = table.Column<bool>(type: "boolean", nullable: false),
                    DisableVersioning = table.Column<bool>(type: "boolean", nullable: false),
                    SaveNewVersionButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SaveNewVersionButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteIndexPageTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreateReturnType = table.Column<int>(type: "integer", nullable: false),
                    EditReturnType = table.Column<int>(type: "integer", nullable: false),
                    PreScript = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PostScript = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IconFileId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateColor = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    BannerFileId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    BackgroundFileId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationSubject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubjectText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    OwnerUserText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RequestedByUserText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PriorityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableCancelButton = table.Column<bool>(type: "boolean", nullable: false),
                    IsCancelReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
                    CancelButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CancelButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsUdfTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    IsCompleteReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
                    NoteNoText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DescriptionText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    HideHeader = table.Column<bool>(type: "boolean", nullable: false),
                    HideSubject = table.Column<bool>(type: "boolean", nullable: false),
                    HideDescription = table.Column<bool>(type: "boolean", nullable: false),
                    HideStartDate = table.Column<bool>(type: "boolean", nullable: false),
                    HideExpiryDate = table.Column<bool>(type: "boolean", nullable: false),
                    HidePriority = table.Column<bool>(type: "boolean", nullable: false),
                    HideOwner = table.Column<bool>(type: "boolean", nullable: false),
                    IsSubjectMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    IsSubjectUnique = table.Column<bool>(type: "boolean", nullable: false),
                    IsDescriptionMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    HideToolbar = table.Column<bool>(type: "boolean", nullable: false),
                    HideBanner = table.Column<bool>(type: "boolean", nullable: false),
                    AllowPastStartDate = table.Column<bool>(type: "boolean", nullable: false),
                    EnablePrintButton = table.Column<bool>(type: "boolean", nullable: false),
                    PrintButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableDataPermission = table.Column<bool>(type: "boolean", nullable: false),
                    DataPermissionColumnId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NumberGenerationType = table.Column<int>(type: "integer", nullable: false),
                    IsNumberNotMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    EnableLegalEntityFilter = table.Column<bool>(type: "boolean", nullable: false),
                    EnablePermission = table.Column<bool>(type: "boolean", nullable: false),
                    EnableInlineComment = table.Column<bool>(type: "boolean", nullable: false),
                    AdhocTaskTemplateIds = table.Column<string[]>(type: "text[]", nullable: true),
                    AdhocServiceTemplateIds = table.Column<string[]>(type: "text[]", nullable: true),
                    AdhocNoteTemplateIds = table.Column<string[]>(type: "text[]", nullable: true),
                    NoteTemplateType = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTemplateLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteTemplateLog_LOV_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NoteTemplateLog_NoteIndexPageTemplate_NoteIndexPageTemplate~",
                        column: x => x.NoteIndexPageTemplateId,
                        principalSchema: "public",
                        principalTable: "NoteIndexPageTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NoteTemplateLog_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteTemplateLog_NoteIndexPageTemplateId",
                schema: "log",
                table: "NoteTemplateLog",
                column: "NoteIndexPageTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTemplateLog_PriorityId",
                schema: "log",
                table: "NoteTemplateLog",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTemplateLog_TemplateId",
                schema: "log",
                table: "NoteTemplateLog",
                column: "TemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteTemplateLog",
                schema: "log");
        }
    }
}
