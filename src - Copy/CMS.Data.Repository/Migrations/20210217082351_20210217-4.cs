using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _202102174 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdhocServiceAddButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AdhocServiceHeaderMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AdhocServiceHeaderText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AdhocTaskAddButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AdhocTaskHeaderMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AdhocTaskHeaderText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "AdminCanEditUdf",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AdminCanSubmitAndAutoComplete",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowTemplateChange",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignToType",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedByQuery",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "AssignedQueryType",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BackButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanAddAdhocTask",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanAddStepService",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanAddStepTask",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditOwner",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanRemoveAdhocService",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanRemoveAdhocTask",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanRemoveStepService",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanRemoveStepTask",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanViewServiceReference",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanViewVersions",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CancelButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CancelButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "ChangeStatusOnStepChange",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientValidationScript",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "CloseButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CloseButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CodeLabelName",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "CollapseHeader",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CompleteButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "CompletionPercentage",
                schema: "public",
                table: "TaskTemplate",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CreateInBackGround",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CreateNewVersionButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreateNewVersionButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultView",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "DelegateButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DelegateButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionLabelName",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "DisableAutomaticDraft",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisableMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableSharing",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisableStepTask",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisplayActionButtonBelow",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentReadyScript",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "DocumentStatus",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DraftButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DraftButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                schema: "public",
                table: "TaskTemplate",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DuplicatedFromId",
                schema: "public",
                table: "TaskTemplate",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditButtonValidationMethod",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "EditButtonVisibilityMethod",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableAdhocService",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableAdhocTask",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableBanner",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableCode",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableDocumentChangeRequest",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableLock",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableParent",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePrintButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableSLAChangeRequest",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableSequenceNo",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableTaskAutoComplete",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableTeamAsOwner",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldSectionMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "FieldSectionText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "HeaderSectionMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "HeaderSectionText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "HideDateAndSLA",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HideDescription",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HideSubject",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeRequesterInOwnerList",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAttachmentRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelReasonRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCodeEditable",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCodeRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCodeUniqueInTemplate",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleteReasonRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfidential",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelegateReasonRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescriptionEditable",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescriptionRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNtsNoManual",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejectionReasonRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReopenReasonRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturnReasonRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSequenceNoEditable",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSequenceNoRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSequenceNoUniqueInTemplate",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubjectEditable",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubjectRequired",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemRating",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeamAsOwnerMandatory",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonForm",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Layout",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "LayoutColumnCount",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LoadExecutionMethod",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ModuleName",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "NotApplicableButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NotificationUrlPattern",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NtsNoLabelName",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "OwnerType",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostSubmitExecutionCode",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PostSubmitExecutionMethod",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PreSubmitExecutionMethod",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrintButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrintButtonVisibilityMethod",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrintMethodName",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "ReSubmitButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReSubmitButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "RejectButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReminderDaysPriorDueDate",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemoveAdhocServiceButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveAdhocServiceConfirmText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveAdhocServiceSuccessMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveAdhocTaskButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveAdhocTaskConfirmText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveAdhocTaskSuccessMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveStepServiceButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveStepServiceConfirmText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveStepServiceSuccessMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveStepTaskButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveStepTaskConfirmText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RemoveStepTaskSuccessMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "ReopenButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReopenButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "ReturnButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReturnButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnUrl",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "RunPostscriptInBackground",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SLA",
                schema: "public",
                table: "TaskTemplate",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SLACalculationMode",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "SaveButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SaveButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "SaveChangesButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SaveChangesButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SaveChangesButtonVisibilityMethod",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SaveNewVersionButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SequenceNoLabelName",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServerValidationScript",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "ServiceOwnerActAsStepTaskAssignee",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceOwnerText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServiceReferenceText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "public",
                table: "TaskTemplate",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusLabelName",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepSectionMessage",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepSectionText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepServiceAddButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepServiceCancelButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepServiceCreationOptionalLabel",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepTaskAddButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepTaskCancelButtonText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepTaskCreationOptionalLabel",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectLabelName",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateMasterId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateOwner",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

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

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CanceledDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompleteReason",
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
                name: "DelegateReason",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DelegatedDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

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

            migrationBuilder.AddColumn<bool>(
                name: "IsTaskAutoCompleteIfSameAssignee",
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

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

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

            migrationBuilder.AddColumn<long>(
                name: "RejectedByUserId",
                schema: "public",
                table: "NtsTask",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReopenReason",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReopenedDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedUserId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnReason",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SLA",
                schema: "public",
                table: "NtsTask",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskNo",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

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
                name: "IX_NtsTask_OwnerUserId",
                schema: "public",
                table: "NtsTask",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_RequestedUserId",
                schema: "public",
                table: "NtsTask",
                column: "RequestedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TemplateId",
                schema: "public",
                table: "NtsTask",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_TaskTemplate_TemplateId",
                schema: "public",
                table: "NtsTask",
                column: "TemplateId",
                principalSchema: "public",
                principalTable: "TaskTemplate",
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
                name: "FK_NtsTask_User_OwnerUserId",
                schema: "public",
                table: "NtsTask",
                column: "OwnerUserId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_TaskTemplate_TemplateId",
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
                name: "FK_NtsTask_User_OwnerUserId",
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
                name: "IX_NtsTask_OwnerUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_RequestedUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "AdhocServiceAddButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocServiceHeaderMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocServiceHeaderText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocTaskAddButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocTaskHeaderMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AdhocTaskHeaderText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AdminCanEditUdf",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AdminCanSubmitAndAutoComplete",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AllowTemplateChange",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AssignToType",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AssignedByQuery",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AssignedQueryType",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "BackButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanAddAdhocTask",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanAddStepService",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanAddStepTask",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanEditOwner",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanRemoveAdhocService",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanRemoveAdhocTask",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanRemoveStepService",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanRemoveStepTask",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanViewServiceReference",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CanViewVersions",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CancelButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CancelButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ChangeStatusOnStepChange",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ClientValidationScript",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CloseButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CloseButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CodeLabelName",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CollapseHeader",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CompleteButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CompletionPercentage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CreateInBackGround",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CreateNewVersionButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "CreateNewVersionButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DefaultView",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DelegateButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DelegateButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DescriptionLabelName",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DisableAutomaticDraft",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DisableMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DisableSharing",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DisableStepTask",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DisplayActionButtonBelow",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DocumentReadyScript",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DocumentStatus",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DraftButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DraftButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DueDate",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DuplicatedFromId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EditButtonValidationMethod",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EditButtonVisibilityMethod",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableAdhocService",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableAdhocTask",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableBanner",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableCode",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableDocumentChangeRequest",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableLock",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableParent",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnablePrintButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableSLAChangeRequest",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableSequenceNo",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableTaskAutoComplete",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableTeamAsOwner",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "FieldSectionMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "FieldSectionText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HeaderSectionMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HeaderSectionText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideDateAndSLA",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideDescription",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideSubject",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IncludeRequesterInOwnerList",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAttachmentRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsCancelReasonRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsCodeEditable",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsCodeRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsCodeUniqueInTemplate",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsCompleteReasonRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsConfidential",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDelegateReasonRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDescriptionEditable",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDescriptionRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsNtsNoManual",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRejectionReasonRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsReopenReasonRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsReturnReasonRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsSequenceNoEditable",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsSequenceNoRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsSequenceNoUniqueInTemplate",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsSubjectEditable",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsSubjectRequired",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsSystemRating",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsTeamAsOwnerMandatory",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "JsonForm",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "Layout",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "LayoutColumnCount",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "LoadExecutionMethod",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ModuleName",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "NotApplicableButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "NotificationUrlPattern",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "NtsNoLabelName",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PostSubmitExecutionCode",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PostSubmitExecutionMethod",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PreSubmitExecutionMethod",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PrintButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PrintButtonVisibilityMethod",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PrintMethodName",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ReSubmitButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ReSubmitButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RejectButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ReminderDaysPriorDueDate",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveAdhocServiceButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveAdhocServiceConfirmText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveAdhocServiceSuccessMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveAdhocTaskButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveAdhocTaskConfirmText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveAdhocTaskSuccessMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveStepServiceButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveStepServiceConfirmText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveStepServiceSuccessMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveStepTaskButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveStepTaskConfirmText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RemoveStepTaskSuccessMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ReopenButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ReopenButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ReturnButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ReturnButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ReturnUrl",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RunPostscriptInBackground",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SLA",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SLACalculationMode",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SaveButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SaveButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SaveChangesButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SaveChangesButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SaveChangesButtonVisibilityMethod",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SaveNewVersionButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SequenceNoLabelName",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ServerValidationScript",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ServiceOwnerActAsStepTaskAssignee",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ServiceOwnerText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ServiceReferenceText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StatusLabelName",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepSectionMessage",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepSectionText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepServiceAddButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepServiceCancelButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepServiceCreationOptionalLabel",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepTaskAddButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepTaskCancelButtonText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepTaskCreationOptionalLabel",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SubjectLabelName",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TemplateMasterId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TemplateOwner",
                schema: "public",
                table: "TaskTemplate");

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
                name: "CancelReason",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "CanceledDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ClosedDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "CompleteReason",
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
                name: "DelegateReason",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DelegatedDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DueDate",
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
                name: "IsTaskAutoCompleteIfSameAssignee",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "LockStatus",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
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
                name: "RejectedByUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "RejectedDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReminderDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReopenReason",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReopenedDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "RequestedUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReturnReason",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "SLA",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TaskNo",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "public",
                table: "NtsTask");
        }
    }
}
