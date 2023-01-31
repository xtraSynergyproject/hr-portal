using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210708_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserSetLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserSet");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleUserLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleUser");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleStatusLabelCode");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleStageParentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleStageParent");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleStageChildLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleStageChild");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserRolePortalLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserRolePortal");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserRolePermissionLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserRolePermission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleDataPermissionLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleDataPermission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserPortalLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserPortal");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserPermissionLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserPermission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserPagePreferenceLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserPagePreference");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserHierarchyPermission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserHierarchyLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserHierarchy");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserGroupUser");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserGroup");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserEntityPermission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UserDataPermissionLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UserDataPermission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "UdfPermissionLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "UdfPermission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TrueComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "TrueComponent");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "TemplateCategory");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TemplateBusinessLogicLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TeamUserLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "TeamUser");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TeamLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TaskIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TaskIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "TaskIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "TableMetadataLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "TableMetadata");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "SubModuleLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "SubModule");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "StartEventComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ServiceIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ServiceIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ServiceIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ResourceLanguage");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "RecruitmentPayElement");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ProjectEmailSetup");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ProcessDesignVariableLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ProcessDesignVariable");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ProcessDesignResultLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ProcessDesignResult");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ProcessDesignLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ProcessDesignComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ProcessDesign");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Portal");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "PermissionLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "PayrollTransaction");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "PayrollRun");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "PayrollBatch");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "PageTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "PageTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "PageNoteLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "PageNote");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "PageMemberLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "PageMemberGroupLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "PageIndexLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "PageIndexColumnLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "PageIndexColumn");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "PageIndex");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "OrganizationDocument");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskTimeEntryLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskTimeEntry");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskSharedLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskShared");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskSequenceLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskSequence");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskPrecedenceLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskPrecedence");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskComment");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskAttachmentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsTag");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsServiceStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsServiceSharedLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsServiceShared");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsServiceSequenceLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsServiceSequence");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsServicePrecedenceLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsServicePrecedence");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsServiceComment");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsNoteStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsNoteStatusTrack");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsNoteSharedLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsNoteShared");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsNoteSequenceLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsNoteSequence");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsNotePrecedenceLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsNotePrecedence");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsNoteCommentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsNoteComment");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsGroupUserGroup");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsGroupTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsGroup");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NtsCategoryLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NtsCategory");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NotificationTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NotificationTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NoteIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "NoteIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "NoteIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ModuleLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "MemberLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "MemberGroupLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ManpowerSummaryComment");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "LOVLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "LOV");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ListOfValue");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "JobDescriptionCriteria");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "JobDescription");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "JobCriteriaTrack");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "JobCriteria");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "JobAdvertisementTrack");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "HiringManagerOrganization");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "HiringManager");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "HierarchyMasterLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "HierarchyMaster");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "HeadOfDepartmentOrganization");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "HeadOfDepartment");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "GrantAccessLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "GrantAccess");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "FormIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "FormIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "FormIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "FormIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "FileLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "File");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "FalseComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "FalseComponent");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ExecutionScriptComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ExecutionScriptComponent");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "EmailSetting");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "EmailLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "EmailComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "EditorTypeLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "EditorLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "DocumentPermission");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "DecisionScriptComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "DecisionScriptComponent");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "DataIntegration");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ContextVariableLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ContextVariable");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ComponentResult");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ComponentParentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ComponentParent");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Component");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "CompleteEventComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "ColumnMetadataLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateSalaryDetail");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateReferences");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateProject");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateLanguageProficiency");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceBySector");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceByOther");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceByNature");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceByJob");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceByCountry");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperience");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateEvaluation");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateEducational");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateDrivingLicense");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateComputerProficiency");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BusinessSection");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRuleNode");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRuleModel");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRuleGroup");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRuleConnector");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRule");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "BusinessLogicComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "BusinessExecutionComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BusinessData");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BusinessArea");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BreResult");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BreMasterTableMetadata");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "BreMasterColumnMetadata");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "AppointmentApprovalRequest");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationStatus");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationStateTrack");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationStateComment");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationState");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationSalaryDetail");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationReferences");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationProject");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationLanguageProficiency");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationJobCriteria");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperienceBySector");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperienceByOther");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperienceByJob");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperienceByCountry");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperience");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationeExperienceByNature");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationEducational");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationDrivingLicense");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationComputerProficiency");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationBeneficiary");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "rec",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "log",
                table: "AdhocTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "PortaalId",
                schema: "public",
                table: "AdhocTaskComponent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserSetLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserSet",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleUserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleUser",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleStatusLabelCode",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleStageParentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleStageParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleStageChildLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleStageChild",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserRolePortalLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserRolePortal",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserRolePermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserRolePermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserRoleDataPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserRoleDataPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserRole",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserPortalLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserPortal",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserPagePreferenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserPagePreference",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserHierarchyPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserHierarchyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserHierarchy",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserGroupUser",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserEntityPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UserDataPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UserDataPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "UdfPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "UdfPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TrueComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "TrueComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TemplateCategoryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "TemplateCategory",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TemplateBusinessLogicLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TeamUserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "TeamUser",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TeamLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Team",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TaskIndexPageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "TaskIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TaskIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "TaskIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "TableMetadataLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "TableMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "SubModuleLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "SubModule",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "StartEventComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ServiceIndexPageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ServiceIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ServiceIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ResourceLanguage",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "RecruitmentPayElement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ProjectEmailSetup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ProcessDesignVariableLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ProcessDesignVariable",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ProcessDesignResultLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ProcessDesignResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ProcessDesignLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ProcessDesignComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ProcessDesign",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "PortalLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Portal",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "PermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Permission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "PayrollTransaction",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "PayrollRun",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "PayrollBatch",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "PageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "PageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "PageNoteLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "PageNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "PageMemberLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "PageMemberGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "PageIndexLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "PageIndexColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "PageIndexColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "PageIndex",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Page",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "OrganizationDocument",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskTimeEntryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskTimeEntry",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskStatusTrackLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskSharedLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskShared",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskPrecedenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskPrecedence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsTaskComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsTaskAttachmentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsTag",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsServiceStatusTrackLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsServiceSharedLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsServiceShared",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsServiceSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsServiceSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsServicePrecedenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsServicePrecedence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsServiceCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsServiceComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsNoteStatusTrackLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsNoteStatusTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsNoteSharedLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsNoteShared",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsNoteSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsNoteSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsNotePrecedenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsNotePrecedence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsNoteCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsNoteComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsGroupUserGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsGroupTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NtsCategoryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NtsCategory",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NotificationTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NotificationTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NotificationLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Notification",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NoteIndexPageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "NoteIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "NoteIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ModuleLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Module",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "MenuGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "MemberLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "MemberGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ManpowerSummaryComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "LOVLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "LOV",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ListOfValue",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "JobDescriptionCriteria",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "JobDescription",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "JobCriteriaTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "JobCriteria",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "JobAdvertisementTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "HiringManagerOrganization",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "HiringManager",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "HierarchyMasterLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "HierarchyMaster",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "HeadOfDepartmentOrganization",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "HeadOfDepartment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "GrantAccessLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "GrantAccess",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "FormTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "FormTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "FormIndexPageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "FormIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "FormIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "FormIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "FileLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "File",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "FalseComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "FalseComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ExecutionScriptComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ExecutionScriptComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "EmailSetting",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "EmailLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "EmailComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Email",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "EditorTypeLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "EditorLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "DocumentPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Document",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "DecisionScriptComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "DecisionScriptComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "DataIntegration",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ContextVariableLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ContextVariable",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ComponentResultLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ComponentResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ComponentParentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ComponentParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Component",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "CompleteEventComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "ColumnMetadataLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "ColumnMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateSalaryDetail",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateReferences",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateProject",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateLanguageProficiency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceBySector",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceByOther",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceByNature",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceByJob",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperienceByCountry",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateExperience",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateEvaluation",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateDrivingLicense",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "CandidateComputerProficiency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BusinessSection",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRuleNode",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRuleModel",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRuleGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRuleConnector",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BusinessRule",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "BusinessLogicComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "BusinessExecutionComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BusinessData",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BusinessArea",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BreResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BreMasterTableMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "BreMasterColumnMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "Batch",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "AppointmentApprovalRequest",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationStatus",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationStateTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationStateComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationState",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationSalaryDetail",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationReferences",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationProject",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationLanguageProficiency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationJobCriteria",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperienceBySector",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperienceByJob",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperienceByCountry",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationExperience",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationeExperienceByNature",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationDrivingLicense",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationComputerProficiency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "ApplicationBeneficiary",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "rec",
                table: "Agency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "log",
                table: "AdhocTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortaalId",
                schema: "public",
                table: "AdhocTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
