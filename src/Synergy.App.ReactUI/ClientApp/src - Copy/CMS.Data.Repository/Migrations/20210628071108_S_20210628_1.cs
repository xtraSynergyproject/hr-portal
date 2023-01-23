﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210628_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserSetLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleUserLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleStageParentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleStageChildLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRolePortalLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRolePermissionLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleDataPermissionLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserPortalLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserPermissionLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserPagePreferenceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserHierarchyLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserDataPermissionLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "UdfPermissionLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TrueComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TemplateCategoryLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TemplateBusinessLogicLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TeamUserLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TeamLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TaskIndexPageTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TaskIndexPageColumnLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "TableMetadataLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "SubModuleLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "StartEventComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ServiceIndexPageTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ServiceIndexPageColumnLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProjectEmailSetupLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProcessDesignVariableLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProcessDesignResultLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProcessDesignLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProcessDesignComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "PortalLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "PermissionLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageNoteLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageMemberLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageMemberGroupLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageIndexLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageIndexColumnLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskTimeEntryLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskStatusTrackLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskSharedLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskSequenceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskPrecedenceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskCommentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskAttachmentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServiceStatusTrackLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServiceSharedLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServiceSequenceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServicePrecedenceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServiceCommentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNoteStatusTrackLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNoteSharedLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNoteSequenceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNotePrecedenceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNoteCommentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsCategoryLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NotificationTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NotificationLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NoteIndexPageTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "NoteIndexPageColumnLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ModuleLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "MenuGroupLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "MemberLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "MemberGroupLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "LOVLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "LegalEntityLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "HierarchyMasterLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "GrantAccessLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "FormTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "FormIndexPageTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "FormIndexPageColumnLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "FileLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "FalseComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ExecutionScriptComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "EmailLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "EmailComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "EditorTypeLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "EditorLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "DecisionScriptComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ContextVariableLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ComponentResultLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ComponentParentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "CompleteEventComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "CompanyLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "ColumnMetadataLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "BusinessLogicComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "BusinessExecutionComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVersionLatest",
                schema: "log",
                table: "AdhocTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserSetLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleUserLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleStageParentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleStageChildLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRolePortalLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRolePermissionLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserRoleDataPermissionLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserPortalLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserPermissionLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserPagePreferenceLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserHierarchyLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UserDataPermissionLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "UdfPermissionLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TrueComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TemplateBusinessLogicLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TeamUserLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TeamLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TaskIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TaskIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "TableMetadataLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "SubModuleLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "StartEventComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ServiceIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ServiceIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProjectEmailSetupLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProcessDesignVariableLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProcessDesignResultLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProcessDesignLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ProcessDesignComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "PermissionLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageNoteLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageMemberLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageMemberGroupLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageIndexLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "PageIndexColumnLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskTimeEntryLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskSharedLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskSequenceLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskPrecedenceLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsTaskAttachmentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServiceStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServiceSharedLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServiceSequenceLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServicePrecedenceLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNoteStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNoteSharedLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNoteSequenceLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNotePrecedenceLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsNoteCommentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NtsCategoryLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NotificationTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NoteIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "NoteIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ModuleLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "MemberLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "MemberGroupLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "LOVLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "HierarchyMasterLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "GrantAccessLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "FormIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "FormIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "FileLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "FalseComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ExecutionScriptComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "EmailLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "EmailComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "EditorTypeLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "EditorLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "DecisionScriptComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ContextVariableLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ComponentParentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "CompleteEventComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "ColumnMetadataLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "BusinessLogicComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "BusinessExecutionComponentLog");

            migrationBuilder.DropColumn(
                name: "IsVersionLatest",
                schema: "log",
                table: "AdhocTaskComponentLog");
        }
    }
}
