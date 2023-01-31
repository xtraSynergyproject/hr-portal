using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210301_S_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_ListOfValue_TaskStatus",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_Team_AssigneeTeamId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_User_AssigneeUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_User_RequestedUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_AssigneeTeamId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_AssigneeUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_RequestedUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TaskStatus",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "AssignToType",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "AssigneeTeamId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "AssigneeUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "CompletedByUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "CompletionDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay1",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay10",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay2",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay3",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay4",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay5",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay6",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay7",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay8",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay9",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue1",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue10",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue2",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue3",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue4",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue5",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue6",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue7",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue8",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue9",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "IsAssignedInTemplate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "IsRead",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "IsTaskAutoComplete",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "LockStatus",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "NtsType",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "PlanOrder",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "RatingComment",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReferenceMasterId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeCode",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "RejectedByUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "RequestedUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TaskStatusCode",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TaskStatusName",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TemplateCode",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay1",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay10",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay2",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay3",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay4",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay5",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay6",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay7",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay8",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay9",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "Priority",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "TemplateAction",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.RenameColumn(
                name: "TextValue9",
                schema: "public",
                table: "NtsTask",
                newName: "TaskTemplateId");

            migrationBuilder.RenameColumn(
                name: "TextValue8",
                schema: "public",
                table: "NtsTask",
                newName: "TaskStatusId");

            migrationBuilder.RenameColumn(
                name: "TextValue7",
                schema: "public",
                table: "NtsTask",
                newName: "TaskPriorityId");

            migrationBuilder.RenameColumn(
                name: "TextValue6",
                schema: "public",
                table: "NtsTask",
                newName: "TaskAssignToTypeId");

            migrationBuilder.RenameColumn(
                name: "TextValue5",
                schema: "public",
                table: "NtsTask",
                newName: "RequestedByUserId");

            migrationBuilder.RenameColumn(
                name: "TextValue4",
                schema: "public",
                table: "NtsTask",
                newName: "ParentTaskId");

            migrationBuilder.RenameColumn(
                name: "TextValue3",
                schema: "public",
                table: "NtsTask",
                newName: "ParentServiceId");

            migrationBuilder.RenameColumn(
                name: "TextValue2",
                schema: "public",
                table: "NtsTask",
                newName: "LockStatusId");

            migrationBuilder.RenameColumn(
                name: "TextValue10",
                schema: "public",
                table: "NtsTask",
                newName: "AssignedToUserId");

            migrationBuilder.RenameColumn(
                name: "TextValue1",
                schema: "public",
                table: "NtsTask",
                newName: "AssignedToTeamId");

            migrationBuilder.RenameColumn(
                name: "IsTaskAutoCompleteIfSameAssignee",
                schema: "public",
                table: "NtsTask",
                newName: "IsStepTaskAutoCompleteIfSameAssignee");

            migrationBuilder.RenameColumn(
                name: "DelegatedDate",
                schema: "public",
                table: "NtsTask",
                newName: "CompletedDate");

            migrationBuilder.AddColumn<bool>(
                name: "CreateTable",
                schema: "public",
                table: "TableMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode1",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode2",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode3",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode4",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentCode5",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue1",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue2",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue3",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue4",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentValue5",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue1",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue2",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue3",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue4",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePickerValue5",
                schema: "public",
                table: "RecTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CanceledDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompleteReason",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelegateReason",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LockStatusId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OwnerTeamId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentServiceId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReopenReason",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReopenedDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedByUserId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnReason",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SLA",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServiceNo",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServiceOwnerTypeId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServicePriorityId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServiceStatusId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServiceTemplateId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedDate",
                schema: "public",
                table: "NtsService",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NoteOwnerTypeId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NotePriorityId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NoteStatusId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "LOV",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LOVType = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    GroupCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ImageId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IconCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
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
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LOV_LOV_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_AssignedToTeamId",
                schema: "public",
                table: "NtsTask",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_AssignedToUserId",
                schema: "public",
                table: "NtsTask",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_LockStatusId",
                schema: "public",
                table: "NtsTask",
                column: "LockStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_ParentServiceId",
                schema: "public",
                table: "NtsTask",
                column: "ParentServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_ParentTaskId",
                schema: "public",
                table: "NtsTask",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_RequestedByUserId",
                schema: "public",
                table: "NtsTask",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "TaskAssignToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TaskPriorityId",
                schema: "public",
                table: "NtsTask",
                column: "TaskPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TaskStatusId",
                schema: "public",
                table: "NtsTask",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TaskTemplateId",
                schema: "public",
                table: "NtsTask",
                column: "TaskTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TemplateId",
                schema: "public",
                table: "NtsTask",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_LockStatusId",
                schema: "public",
                table: "NtsService",
                column: "LockStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_OwnerTeamId",
                schema: "public",
                table: "NtsService",
                column: "OwnerTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_OwnerUserId",
                schema: "public",
                table: "NtsService",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_ParentServiceId",
                schema: "public",
                table: "NtsService",
                column: "ParentServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_RequestedByUserId",
                schema: "public",
                table: "NtsService",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_ServiceOwnerTypeId",
                schema: "public",
                table: "NtsService",
                column: "ServiceOwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_ServicePriorityId",
                schema: "public",
                table: "NtsService",
                column: "ServicePriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_ServiceStatusId",
                schema: "public",
                table: "NtsService",
                column: "ServiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_ServiceTemplateId",
                schema: "public",
                table: "NtsService",
                column: "ServiceTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_TemplateId",
                schema: "public",
                table: "NtsService",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNote_NoteOwnerTypeId",
                schema: "public",
                table: "NtsNote",
                column: "NoteOwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNote_NotePriorityId",
                schema: "public",
                table: "NtsNote",
                column: "NotePriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNote_NoteStatusId",
                schema: "public",
                table: "NtsNote",
                column: "NoteStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LOV_ParentId",
                schema: "public",
                table: "LOV",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNote_LOV_NoteOwnerTypeId",
                schema: "public",
                table: "NtsNote",
                column: "NoteOwnerTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNote_LOV_NotePriorityId",
                schema: "public",
                table: "NtsNote",
                column: "NotePriorityId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNote_LOV_NoteStatusId",
                schema: "public",
                table: "NtsNote",
                column: "NoteStatusId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_LOV_LockStatusId",
                schema: "public",
                table: "NtsService",
                column: "LockStatusId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_LOV_ServiceOwnerTypeId",
                schema: "public",
                table: "NtsService",
                column: "ServiceOwnerTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_LOV_ServicePriorityId",
                schema: "public",
                table: "NtsService",
                column: "ServicePriorityId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_LOV_ServiceStatusId",
                schema: "public",
                table: "NtsService",
                column: "ServiceStatusId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_NtsService_ParentServiceId",
                schema: "public",
                table: "NtsService",
                column: "ParentServiceId",
                principalSchema: "public",
                principalTable: "NtsService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_ServiceTemplate_ServiceTemplateId",
                schema: "public",
                table: "NtsService",
                column: "ServiceTemplateId",
                principalSchema: "public",
                principalTable: "ServiceTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_Team_OwnerTeamId",
                schema: "public",
                table: "NtsService",
                column: "OwnerTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_Template_TemplateId",
                schema: "public",
                table: "NtsService",
                column: "TemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_User_OwnerUserId",
                schema: "public",
                table: "NtsService",
                column: "OwnerUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_User_RequestedByUserId",
                schema: "public",
                table: "NtsService",
                column: "RequestedByUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_LockStatusId",
                schema: "public",
                table: "NtsTask",
                column: "LockStatusId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "TaskAssignToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_TaskPriorityId",
                schema: "public",
                table: "NtsTask",
                column: "TaskPriorityId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_TaskStatusId",
                schema: "public",
                table: "NtsTask",
                column: "TaskStatusId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_NtsService_ParentServiceId",
                schema: "public",
                table: "NtsTask",
                column: "ParentServiceId",
                principalSchema: "public",
                principalTable: "NtsService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_NtsTask_ParentTaskId",
                schema: "public",
                table: "NtsTask",
                column: "ParentTaskId",
                principalSchema: "public",
                principalTable: "NtsTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_TaskTemplate_TaskTemplateId",
                schema: "public",
                table: "NtsTask",
                column: "TaskTemplateId",
                principalSchema: "public",
                principalTable: "TaskTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_Team_AssignedToTeamId",
                schema: "public",
                table: "NtsTask",
                column: "AssignedToTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_Template_TemplateId",
                schema: "public",
                table: "NtsTask",
                column: "TemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_User_AssignedToUserId",
                schema: "public",
                table: "NtsTask",
                column: "AssignedToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_User_RequestedByUserId",
                schema: "public",
                table: "NtsTask",
                column: "RequestedByUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsNote_LOV_NoteOwnerTypeId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsNote_LOV_NotePriorityId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsNote_LOV_NoteStatusId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_LOV_LockStatusId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_LOV_ServiceOwnerTypeId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_LOV_ServicePriorityId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_LOV_ServiceStatusId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_NtsService_ParentServiceId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_ServiceTemplate_ServiceTemplateId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_Team_OwnerTeamId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_Template_TemplateId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_User_OwnerUserId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_User_RequestedByUserId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_LockStatusId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_TaskPriorityId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_TaskStatusId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_NtsService_ParentServiceId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_NtsTask_ParentTaskId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_TaskTemplate_TaskTemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_Team_AssignedToTeamId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_Template_TemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_User_AssignedToUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_User_RequestedByUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropTable(
                name: "LOV",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_AssignedToTeamId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_AssignedToUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_LockStatusId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_ParentServiceId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_ParentTaskId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_RequestedByUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TaskPriorityId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TaskStatusId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TaskTemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_LockStatusId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_OwnerTeamId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_OwnerUserId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_ParentServiceId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_RequestedByUserId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_ServiceOwnerTypeId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_ServicePriorityId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_ServiceStatusId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_ServiceTemplateId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_TemplateId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsNote_NoteOwnerTypeId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropIndex(
                name: "IX_NtsNote_NotePriorityId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropIndex(
                name: "IX_NtsNote_NoteStatusId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "CreateTable",
                schema: "public",
                table: "TableMetadata");

            migrationBuilder.DropColumn(
                name: "AttachmentCode1",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentCode2",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentCode3",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentCode4",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentCode5",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue1",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue2",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue3",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue4",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "AttachmentValue5",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue1",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue2",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue3",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue4",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "DatePickerValue5",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "CanceledDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ClosedDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "CompleteReason",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "CompletedDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "DelegateReason",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "DueDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "LockStatusId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "OwnerTeamId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ParentServiceId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "RejectedDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ReminderDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ReopenReason",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ReopenedDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "RequestedByUserId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ReturnReason",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "SLA",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ServiceNo",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ServiceOwnerTypeId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ServicePriorityId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ServiceStatusId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ServiceTemplateId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "NoteOwnerTypeId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "NotePriorityId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "NoteStatusId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.RenameColumn(
                name: "TaskTemplateId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue9");

            migrationBuilder.RenameColumn(
                name: "TaskStatusId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue8");

            migrationBuilder.RenameColumn(
                name: "TaskPriorityId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue7");

            migrationBuilder.RenameColumn(
                name: "TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue6");

            migrationBuilder.RenameColumn(
                name: "RequestedByUserId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue5");

            migrationBuilder.RenameColumn(
                name: "ParentTaskId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue4");

            migrationBuilder.RenameColumn(
                name: "ParentServiceId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue3");

            migrationBuilder.RenameColumn(
                name: "LockStatusId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue2");

            migrationBuilder.RenameColumn(
                name: "IsStepTaskAutoCompleteIfSameAssignee",
                schema: "public",
                table: "NtsTask",
                newName: "IsTaskAutoCompleteIfSameAssignee");

            migrationBuilder.RenameColumn(
                name: "CompletedDate",
                schema: "public",
                table: "NtsTask",
                newName: "DelegatedDate");

            migrationBuilder.RenameColumn(
                name: "AssignedToUserId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue10");

            migrationBuilder.RenameColumn(
                name: "AssignedToTeamId",
                schema: "public",
                table: "NtsTask",
                newName: "TextValue1");

            migrationBuilder.AddColumn<int>(
                name: "AssignToType",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssigneeTeamId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AssigneeUserId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<long>(
                name: "CompletedByUserId",
                schema: "public",
                table: "NtsTask",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay1",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay10",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay2",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay3",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay4",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay5",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay6",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay7",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay8",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay9",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue1",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue10",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue2",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue3",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue4",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue5",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue6",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue7",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue8",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue9",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedInTemplate",
                schema: "public",
                table: "NtsTask",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                schema: "public",
                table: "NtsTask",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaskAutoComplete",
                schema: "public",
                table: "NtsTask",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LockStatus",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NtsType",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PlanOrder",
                schema: "public",
                table: "NtsTask",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RatingComment",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceMasterId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceTypeCode",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTypeId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<long>(
                name: "RejectedByUserId",
                schema: "public",
                table: "NtsTask",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedUserId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TaskStatus",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TaskStatusCode",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TaskStatusName",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateCode",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay1",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay10",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay2",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay3",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay4",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay5",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay6",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay7",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay8",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay9",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                schema: "public",
                table: "NtsNote",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TemplateAction",
                schema: "public",
                table: "NtsNote",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_AssigneeTeamId",
                schema: "public",
                table: "NtsTask",
                column: "AssigneeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_AssigneeUserId",
                schema: "public",
                table: "NtsTask",
                column: "AssigneeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_RequestedUserId",
                schema: "public",
                table: "NtsTask",
                column: "RequestedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TaskStatus",
                schema: "public",
                table: "NtsTask",
                column: "TaskStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_ListOfValue_TaskStatus",
                schema: "public",
                table: "NtsTask",
                column: "TaskStatus",
                principalSchema: "rec",
                principalTable: "ListOfValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_Team_AssigneeTeamId",
                schema: "public",
                table: "NtsTask",
                column: "AssigneeTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_User_AssigneeUserId",
                schema: "public",
                table: "NtsTask",
                column: "AssigneeUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_User_RequestedUserId",
                schema: "public",
                table: "NtsTask",
                column: "RequestedUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
