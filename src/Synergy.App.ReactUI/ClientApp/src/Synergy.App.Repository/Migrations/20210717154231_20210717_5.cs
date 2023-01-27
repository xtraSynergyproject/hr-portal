using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210717_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NtsNoteLog",
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
                    NoteNo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteSubject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteDescription = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NoteStatusId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotePriorityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteOwnerTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RequestedByUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    OwnerUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentNoteId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentTaskId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteActionId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CancelReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserRating = table.Column<double>(type: "double precision", nullable: true),
                    CloseComment = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ClosedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsVersioning = table.Column<bool>(type: "boolean", nullable: false),
                    ReferenceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReferenceType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsNoteLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_LOV_NoteActionId",
                        column: x => x.NoteActionId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_LOV_NoteOwnerTypeId",
                        column: x => x.NoteOwnerTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_LOV_NotePriorityId",
                        column: x => x.NotePriorityId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_LOV_NoteStatusId",
                        column: x => x.NoteStatusId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_NoteTemplate_NoteTemplateId",
                        column: x => x.NoteTemplateId,
                        principalSchema: "public",
                        principalTable: "NoteTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_NtsNote_ParentNoteId",
                        column: x => x.ParentNoteId,
                        principalSchema: "public",
                        principalTable: "NtsNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_NtsService_ParentServiceId",
                        column: x => x.ParentServiceId,
                        principalSchema: "public",
                        principalTable: "NtsService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_NtsTask_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalSchema: "public",
                        principalTable: "NtsTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_User_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteLog_User_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NtsServiceLog",
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
                    ServiceNo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServiceSubject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServiceDescription = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ServiceSLA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ActualSLA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ServiceStatusId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServiceActionId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServicePriorityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReopenedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CloseComment = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserRating = table.Column<double>(type: "double precision", nullable: true),
                    ServiceOwnerTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RejectionReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReturnReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReopenReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CancelReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DelegateReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UdfTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UdfNoteId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UdfNoteTableId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServiceTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RequestedByUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    OwnerUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    OwnerTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LockStatusId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsVersioning = table.Column<bool>(type: "boolean", nullable: false),
                    ReferenceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReferenceType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsServiceLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_LOV_LockStatusId",
                        column: x => x.LockStatusId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_LOV_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_LOV_ServiceOwnerTypeId",
                        column: x => x.ServiceOwnerTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_LOV_ServicePriorityId",
                        column: x => x.ServicePriorityId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_LOV_ServiceStatusId",
                        column: x => x.ServiceStatusId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_NtsNote_UdfNoteId",
                        column: x => x.UdfNoteId,
                        principalSchema: "public",
                        principalTable: "NtsNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_NtsService_ParentServiceId",
                        column: x => x.ParentServiceId,
                        principalSchema: "public",
                        principalTable: "NtsService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_ServiceTemplate_ServiceTemplateId",
                        column: x => x.ServiceTemplateId,
                        principalSchema: "public",
                        principalTable: "ServiceTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_Team_OwnerTeamId",
                        column: x => x.OwnerTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_Template_UdfTemplateId",
                        column: x => x.UdfTemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_User_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceLog_User_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NtsTaskLog",
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
                    TaskNo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskSubject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskDescription = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskType = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TaskSLA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ActualSLA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PlanDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TaskStatusId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskActionId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskPriorityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReopenedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReturnReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReopenReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CancelReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DelegateReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CloseComment = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserRating = table.Column<double>(type: "double precision", nullable: true),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UdfTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UdfNoteId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UdfNoteTableId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RequestedByUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    OwnerUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskOwnerTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsStepTaskAutoCompleteIfSameAssignee = table.Column<bool>(type: "boolean", nullable: false),
                    LockStatusId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentTaskId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterLevelId = table.Column<int>(type: "integer", nullable: true),
                    IsVersioning = table.Column<bool>(type: "boolean", nullable: false),
                    StepTaskComponentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReferenceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReferenceType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsTaskLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_LOV_AssignedToTypeId",
                        column: x => x.AssignedToTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_LOV_LockStatusId",
                        column: x => x.LockStatusId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_LOV_TaskActionId",
                        column: x => x.TaskActionId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_LOV_TaskOwnerTypeId",
                        column: x => x.TaskOwnerTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_LOV_TaskPriorityId",
                        column: x => x.TaskPriorityId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_LOV_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_NtsNote_UdfNoteId",
                        column: x => x.UdfNoteId,
                        principalSchema: "public",
                        principalTable: "NtsNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_TaskTemplate_TaskTemplateId",
                        column: x => x.TaskTemplateId,
                        principalSchema: "public",
                        principalTable: "TaskTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_Team_AssignedToTeamId",
                        column: x => x.AssignedToTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_Template_UdfTemplateId",
                        column: x => x.UdfTemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_User_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_User_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskLog_User_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_NoteActionId",
                schema: "log",
                table: "NtsNoteLog",
                column: "NoteActionId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_NoteOwnerTypeId",
                schema: "log",
                table: "NtsNoteLog",
                column: "NoteOwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_NotePriorityId",
                schema: "log",
                table: "NtsNoteLog",
                column: "NotePriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_NoteStatusId",
                schema: "log",
                table: "NtsNoteLog",
                column: "NoteStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_NoteTemplateId",
                schema: "log",
                table: "NtsNoteLog",
                column: "NoteTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_OwnerUserId",
                schema: "log",
                table: "NtsNoteLog",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_ParentNoteId",
                schema: "log",
                table: "NtsNoteLog",
                column: "ParentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_ParentServiceId",
                schema: "log",
                table: "NtsNoteLog",
                column: "ParentServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_ParentTaskId",
                schema: "log",
                table: "NtsNoteLog",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_RequestedByUserId",
                schema: "log",
                table: "NtsNoteLog",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_TemplateId",
                schema: "log",
                table: "NtsNoteLog",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_LockStatusId",
                schema: "log",
                table: "NtsServiceLog",
                column: "LockStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_OwnerTeamId",
                schema: "log",
                table: "NtsServiceLog",
                column: "OwnerTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_OwnerUserId",
                schema: "log",
                table: "NtsServiceLog",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_ParentServiceId",
                schema: "log",
                table: "NtsServiceLog",
                column: "ParentServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_RequestedByUserId",
                schema: "log",
                table: "NtsServiceLog",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_ServiceActionId",
                schema: "log",
                table: "NtsServiceLog",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_ServiceOwnerTypeId",
                schema: "log",
                table: "NtsServiceLog",
                column: "ServiceOwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_ServicePriorityId",
                schema: "log",
                table: "NtsServiceLog",
                column: "ServicePriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_ServiceStatusId",
                schema: "log",
                table: "NtsServiceLog",
                column: "ServiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_ServiceTemplateId",
                schema: "log",
                table: "NtsServiceLog",
                column: "ServiceTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_TemplateId",
                schema: "log",
                table: "NtsServiceLog",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_UdfNoteId",
                schema: "log",
                table: "NtsServiceLog",
                column: "UdfNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_UdfTemplateId",
                schema: "log",
                table: "NtsServiceLog",
                column: "UdfTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_AssignedToTeamId",
                schema: "log",
                table: "NtsTaskLog",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_AssignedToTypeId",
                schema: "log",
                table: "NtsTaskLog",
                column: "AssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_AssignedToUserId",
                schema: "log",
                table: "NtsTaskLog",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_LockStatusId",
                schema: "log",
                table: "NtsTaskLog",
                column: "LockStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_OwnerUserId",
                schema: "log",
                table: "NtsTaskLog",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_RequestedByUserId",
                schema: "log",
                table: "NtsTaskLog",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_TaskActionId",
                schema: "log",
                table: "NtsTaskLog",
                column: "TaskActionId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_TaskOwnerTypeId",
                schema: "log",
                table: "NtsTaskLog",
                column: "TaskOwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_TaskPriorityId",
                schema: "log",
                table: "NtsTaskLog",
                column: "TaskPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_TaskStatusId",
                schema: "log",
                table: "NtsTaskLog",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_TaskTemplateId",
                schema: "log",
                table: "NtsTaskLog",
                column: "TaskTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_TemplateId",
                schema: "log",
                table: "NtsTaskLog",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_UdfNoteId",
                schema: "log",
                table: "NtsTaskLog",
                column: "UdfNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_UdfTemplateId",
                schema: "log",
                table: "NtsTaskLog",
                column: "UdfTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NtsNoteLog",
                schema: "log");

            migrationBuilder.DropTable(
                name: "NtsServiceLog",
                schema: "log");

            migrationBuilder.DropTable(
                name: "NtsTaskLog",
                schema: "log");
        }
    }
}
