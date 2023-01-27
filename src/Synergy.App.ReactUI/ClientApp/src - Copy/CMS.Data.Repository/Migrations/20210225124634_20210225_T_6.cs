using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210225_T_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecTask",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskNo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReferenceMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SLA = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskStatus = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskStatusCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskStatusName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DelegatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReopenedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RatingComment = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignToType = table.Column<int>(type: "integer", nullable: true),
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
                    OwnerUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssigneeUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RequestedUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssigneeTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsTaskAutoComplete = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssignedInTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    ReferenceTypeCode = table.Column<int>(type: "integer", nullable: true),
                    ReferenceTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompletedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    RejectedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsTaskAutoCompleteIfSameAssignee = table.Column<bool>(type: "boolean", nullable: false),
                    LockStatus = table.Column<int>(type: "integer", nullable: true),
                    PlanOrder = table.Column<long>(type: "bigint", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: true),
                    TextValue1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue6 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay6 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue6 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay6 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue7 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay7 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue7 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay7 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue8 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay8 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue8 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay8 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue9 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay9 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue9 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay9 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue10 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay10 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue10 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay10 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsType = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_RecTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecTask_ListOfValue_TaskStatus",
                        column: x => x.TaskStatus,
                        principalSchema: "rec",
                        principalTable: "ListOfValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecTask_Team_AssigneeTeamId",
                        column: x => x.AssigneeTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecTask_User_AssigneeUserId",
                        column: x => x.AssigneeUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecTask_User_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecTask_User_RequestedUserId",
                        column: x => x.RequestedUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecTaskTemplate",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    HeaderSectionText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    HeaderSectionMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    FieldSectionText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    FieldSectionMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepSectionText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepSectionMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CanAddStepTask = table.Column<bool>(type: "boolean", nullable: false),
                    StepTaskCreationOptionalLabel = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepTaskAddButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepTaskCancelButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CanRemoveStepTask = table.Column<bool>(type: "boolean", nullable: false),
                    RemoveStepTaskButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RemoveStepTaskConfirmText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RemoveStepTaskSuccessMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableAdhocTask = table.Column<bool>(type: "boolean", nullable: false),
                    CanAddAdhocTask = table.Column<bool>(type: "boolean", nullable: false),
                    AdhocTaskHeaderText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AdhocTaskAddButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AdhocTaskHeaderMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DraftButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SaveButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RejectButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReturnButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReopenButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DelegateButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CancelButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    BackButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreateNewVersionButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SaveChangesButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SaveNewVersionButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DraftButton = table.Column<bool>(type: "boolean", nullable: false),
                    CreateNewVersionButton = table.Column<bool>(type: "boolean", nullable: false),
                    SaveButton = table.Column<bool>(type: "boolean", nullable: false),
                    CanViewVersions = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayActionButtonBelow = table.Column<bool>(type: "boolean", nullable: true),
                    SaveChangesButton = table.Column<bool>(type: "boolean", nullable: false),
                    CompleteButton = table.Column<bool>(type: "boolean", nullable: false),
                    IsCompleteReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
                    RejectButton = table.Column<bool>(type: "boolean", nullable: false),
                    NotApplicableButton = table.Column<bool>(type: "boolean", nullable: false),
                    IsRejectionReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
                    ReturnButton = table.Column<bool>(type: "boolean", nullable: false),
                    ReopenButton = table.Column<bool>(type: "boolean", nullable: true),
                    IsReopenReasonRequired = table.Column<bool>(type: "boolean", nullable: true),
                    IsReturnReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
                    DelegateButton = table.Column<bool>(type: "boolean", nullable: false),
                    IsDelegateReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
                    CancelButton = table.Column<bool>(type: "boolean", nullable: false),
                    IsCancelReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
                    BackButton = table.Column<bool>(type: "boolean", nullable: false),
                    CloseButton = table.Column<bool>(type: "boolean", nullable: false),
                    CloseButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SLA = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SLACalculationMode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReminderDaysPriorDueDate = table.Column<int>(type: "integer", nullable: true),
                    IsSystemRating = table.Column<bool>(type: "boolean", nullable: false),
                    IsConfidential = table.Column<bool>(type: "boolean", nullable: false),
                    CollapseHeader = table.Column<bool>(type: "boolean", nullable: false),
                    DuplicatedFromId = table.Column<long>(type: "bigint", nullable: true),
                    AssignToType = table.Column<int>(type: "integer", nullable: true),
                    OwnerType = table.Column<int>(type: "integer", nullable: true),
                    CanEditOwner = table.Column<bool>(type: "boolean", nullable: false),
                    AssignedQueryType = table.Column<int>(type: "integer", nullable: true),
                    AssignedToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedByQuery = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LayoutColumnCount = table.Column<int>(type: "integer", nullable: false),
                    DocumentStatus = table.Column<int>(type: "integer", nullable: true),
                    DisableMessage = table.Column<bool>(type: "boolean", nullable: false),
                    NtsNoLabelName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsNtsNoManual = table.Column<bool>(type: "boolean", nullable: true),
                    SubjectLabelName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsSubjectRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsSubjectEditable = table.Column<bool>(type: "boolean", nullable: false),
                    DescriptionLabelName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsDescriptionRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsDescriptionEditable = table.Column<bool>(type: "boolean", nullable: false),
                    ClientValidationScript = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DocumentReadyScript = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServerValidationScript = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LoadExecutionMethod = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PostSubmitExecutionMethod = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PostSubmitExecutionCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RunPostscriptInBackground = table.Column<bool>(type: "boolean", nullable: true),
                    DisableAutomaticDraft = table.Column<bool>(type: "boolean", nullable: true),
                    EditButtonValidationMethod = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PreSubmitExecutionMethod = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SaveChangesButtonVisibilityMethod = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EditButtonVisibilityMethod = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PrintButtonVisibilityMethod = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CanViewServiceReference = table.Column<bool>(type: "boolean", nullable: false),
                    ServiceReferenceText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationUrlPattern = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ModuleName = table.Column<int>(type: "integer", nullable: false),
                    StatusLabelName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableTaskAutoComplete = table.Column<bool>(type: "boolean", nullable: false),
                    EnableSLAChangeRequest = table.Column<bool>(type: "boolean", nullable: false),
                    CanRemoveAdhocTask = table.Column<bool>(type: "boolean", nullable: false),
                    RemoveAdhocTaskButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RemoveAdhocTaskConfirmText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RemoveAdhocTaskSuccessMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CanAddStepService = table.Column<bool>(type: "boolean", nullable: false),
                    StepServiceCreationOptionalLabel = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepServiceAddButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepServiceCancelButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CanRemoveStepService = table.Column<bool>(type: "boolean", nullable: false),
                    RemoveStepServiceButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RemoveStepServiceConfirmText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RemoveStepServiceSuccessMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableAdhocService = table.Column<bool>(type: "boolean", nullable: false),
                    AdhocServiceHeaderText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AdhocServiceAddButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AdhocServiceHeaderMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CanRemoveAdhocService = table.Column<bool>(type: "boolean", nullable: false),
                    RemoveAdhocServiceButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RemoveAdhocServiceConfirmText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RemoveAdhocServiceSuccessMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    HideDateAndSLA = table.Column<bool>(type: "boolean", nullable: true),
                    IsAttachmentRequired = table.Column<bool>(type: "boolean", nullable: true),
                    ChangeStatusOnStepChange = table.Column<bool>(type: "boolean", nullable: true),
                    ServiceOwnerText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IncludeRequesterInOwnerList = table.Column<bool>(type: "boolean", nullable: true),
                    ServiceOwnerActAsStepTaskAssignee = table.Column<bool>(type: "boolean", nullable: true),
                    CreateInBackGround = table.Column<bool>(type: "boolean", nullable: true),
                    DisableStepTask = table.Column<bool>(type: "boolean", nullable: true),
                    AdminCanEditUdf = table.Column<bool>(type: "boolean", nullable: true),
                    AdminCanSubmitAndAutoComplete = table.Column<bool>(type: "boolean", nullable: true),
                    EnableTeamAsOwner = table.Column<bool>(type: "boolean", nullable: true),
                    IsTeamAsOwnerMandatory = table.Column<bool>(type: "boolean", nullable: true),
                    Layout = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReturnUrl = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    HideSubject = table.Column<bool>(type: "boolean", nullable: true),
                    HideDescription = table.Column<bool>(type: "boolean", nullable: true),
                    EnablePrintButton = table.Column<bool>(type: "boolean", nullable: true),
                    PrintButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PrintMethodName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableLock = table.Column<bool>(type: "boolean", nullable: true),
                    ReSubmitButton = table.Column<bool>(type: "boolean", nullable: true),
                    ReSubmitButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableBanner = table.Column<bool>(type: "boolean", nullable: true),
                    AllowTemplateChange = table.Column<bool>(type: "boolean", nullable: true),
                    EnableCode = table.Column<bool>(type: "boolean", nullable: true),
                    IsCodeRequired = table.Column<bool>(type: "boolean", nullable: true),
                    IsCodeUniqueInTemplate = table.Column<bool>(type: "boolean", nullable: true),
                    IsCodeEditable = table.Column<bool>(type: "boolean", nullable: true),
                    CodeLabelName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableSequenceNo = table.Column<bool>(type: "boolean", nullable: true),
                    IsSequenceNoRequired = table.Column<bool>(type: "boolean", nullable: true),
                    IsSequenceNoUniqueInTemplate = table.Column<bool>(type: "boolean", nullable: true),
                    IsSequenceNoEditable = table.Column<bool>(type: "boolean", nullable: true),
                    SequenceNoLabelName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DisableSharing = table.Column<bool>(type: "boolean", nullable: true),
                    CompletionPercentage = table.Column<double>(type: "double precision", nullable: true),
                    DefaultView = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableDocumentChangeRequest = table.Column<bool>(type: "boolean", nullable: true),
                    TemplateOwner = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableParent = table.Column<bool>(type: "boolean", nullable: true),
                    JsonForm = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableIndexPage = table.Column<bool>(type: "boolean", nullable: false),
                    EnableTaskNumberManual = table.Column<bool>(type: "boolean", nullable: false),
                    EnableSaveAsDraft = table.Column<bool>(type: "boolean", nullable: false),
                    SaveAsDraftText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SaveAsDraftCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubmitButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubmitButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableRejectButton = table.Column<bool>(type: "boolean", nullable: false),
                    RejectButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableBackButton = table.Column<bool>(type: "boolean", nullable: false),
                    BackButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EnableAttachment = table.Column<bool>(type: "boolean", nullable: false),
                    EnableComment = table.Column<bool>(type: "boolean", nullable: false),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskIndexPageTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplay1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxLink1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType1 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay1 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay1 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay1 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay1 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType2 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay2 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay2 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay2 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay2 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType3 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay3 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay3 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay3 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay3 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType4 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay4 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay4 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay4 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay4 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType5 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay5 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay5 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay5 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay5 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay6 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType6 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay6 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay6 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay6 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod6 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay6 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay6 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay7 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType7 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay7 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay7 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay7 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod7 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay7 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay7 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay8 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType8 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay8 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay8 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay8 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod8 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay8 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay8 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay9 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType9 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay9 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay9 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay9 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod9 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay9 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay9 = table.Column<bool>(type: "boolean", nullable: false),
                    TextBoxDisplay10 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextBoxDisplayType10 = table.Column<int>(type: "integer", nullable: true),
                    IsRequiredTextBoxDisplay10 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableTextBoxDisplay10 = table.Column<bool>(type: "boolean", nullable: false),
                    IsDropdownDisplay10 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValueMethod10 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsRequiredDropdownDisplay10 = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssigneeAvailableDropdownDisplay10 = table.Column<bool>(type: "boolean", nullable: false),
                    NtsType = table.Column<int>(type: "integer", nullable: true),
                    StepTemplateIds = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServiceDetailsHeight = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_RecTaskTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecTaskTemplate_TaskIndexPageTemplate_TaskIndexPageTemplate~",
                        column: x => x.TaskIndexPageTemplateId,
                        principalSchema: "public",
                        principalTable: "TaskIndexPageTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecTaskTemplate_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecTask_AssigneeTeamId",
                schema: "public",
                table: "RecTask",
                column: "AssigneeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RecTask_AssigneeUserId",
                schema: "public",
                table: "RecTask",
                column: "AssigneeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecTask_OwnerUserId",
                schema: "public",
                table: "RecTask",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecTask_RequestedUserId",
                schema: "public",
                table: "RecTask",
                column: "RequestedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecTask_TaskStatus",
                schema: "public",
                table: "RecTask",
                column: "TaskStatus");

            migrationBuilder.CreateIndex(
                name: "IX_RecTaskTemplate_TaskIndexPageTemplateId",
                schema: "public",
                table: "RecTaskTemplate",
                column: "TaskIndexPageTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RecTaskTemplate_TemplateId",
                schema: "public",
                table: "RecTaskTemplate",
                column: "TemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecTask",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RecTaskTemplate",
                schema: "public");
        }
    }
}
