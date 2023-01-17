using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210531_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserSetLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserSet",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleUserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleUser",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleStatusLabelCode",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleStageParentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleStageParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleStageChildLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleStageChild",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRolePortalLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRolePortal",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRolePermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRolePermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleDataPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleDataPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRole",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserPortalLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserPortal",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserPagePreferenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserPagePreference",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserHierarchyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserHierarchy",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserGroupUser",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UserDataPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UserDataPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "UdfPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "UdfPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TrueComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "TrueComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TemplateCategoryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "TemplateCategory",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TemplateBusinessLogicLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TeamUserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "TeamUser",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TeamLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Team",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TaskIndexPageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "TaskIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TaskIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "TaskIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "TableMetadataLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "TableMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "SubModuleLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "SubModule",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "StartEventComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ServiceIndexPageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ServiceIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ServiceIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "RecTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "RecTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "RecruitmentPayElement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ProjectEmailSetup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ProcessDesignVariableLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ProcessDesignVariable",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ProcessDesignResultLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ProcessDesignResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ProcessDesignLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ProcessDesignComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ProcessDesign",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "PortalLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Portal",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "PermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Permission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "PayrollTransaction",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "PayrollRun",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "PayrollBatch",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "PageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "PageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "PageNoteLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "PageNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "PageMemberLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "PageMemberGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "PageIndexLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "PageIndexColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "PageIndexColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "PageIndex",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Page",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskTimeEntryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskTimeEntry",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskStatusTrackLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskSharedLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskShared",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskPrecedenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskPrecedence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskAttachmentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServiceStatusTrackLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServiceSharedLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsServiceShared",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServiceSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsServiceSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServicePrecedenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsServicePrecedence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServiceCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsServiceComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNoteStatusTrackLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNoteStatusTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNoteSharedLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNoteShared",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNoteSequenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNoteSequence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNotePrecedenceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNotePrecedence",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNoteCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNoteComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsGroupUserGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsGroupTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsCategoryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsCategory",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NotificationTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NotificationTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NotificationLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Notification",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NoteIndexPageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NoteIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "NoteIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ModuleLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Module",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "MenuGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "MemberLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "MemberGroupLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ManpowerSummaryComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "LOVLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "LOV",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ListOfValue",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobDescriptionCriteria",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobDescription",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobCriteriaTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobCriteria",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobAdvertisementTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "HiringManagerOrganization",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "HiringManager",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "HierarchyMasterLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "HierarchyMaster",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "HeadOfDepartmentOrganization",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "HeadOfDepartment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "FormTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "FormTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "FormIndexPageTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "FormIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "FormIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "FormIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "FileLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "File",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "FalseComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "FalseComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ExecutionScriptComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ExecutionScriptComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "EmailSetting",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "EmailLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "EmailComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Email",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "EditorTypeLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "EditorLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Document",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "DecisionScriptComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "DecisionScriptComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "DataIntegration",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ContextVariableLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ContextVariable",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ComponentResultLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ComponentResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ComponentParentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ComponentParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Component",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "CompleteEventComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "ColumnMetadataLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "ColumnMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateSalaryDetail",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateReferences",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateProject",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateLanguageProficiency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceBySector",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceByOther",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceByNature",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceByJob",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceByCountry",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperience",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateEvaluation",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateDrivingLicense",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateComputerProficiency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessSection",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRuleNode",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRuleModel",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRuleGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRuleConnector",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRule",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "BusinessLogicComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "BusinessExecutionComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessData",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessArea",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BreResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BreMasterTableMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "BreMasterColumnMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "Batch",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "AppointmentApprovalRequest",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationStatus",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationStateTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationStateComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationState",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationSalaryDetail",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationReferences",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationProject",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationLanguageProficiency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationJobCriteria",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperienceBySector",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperienceByJob",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperienceByCountry",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperience",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationeExperienceByNature",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationDrivingLicense",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationComputerProficiency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationBeneficiary",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "rec",
                table: "Agency",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "AdhocTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "public",
                table: "AdhocTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserSetLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserSet");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleUserLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleUser");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleStatusLabelCode");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleStageParentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleStageParent");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleStageChildLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleStageChild");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRolePortalLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRolePortal");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRolePermissionLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRolePermission");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserRoleDataPermissionLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRoleDataPermission");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserPortalLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserPortal");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserPermissionLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserPermission");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserPagePreferenceLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserPagePreference");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserHierarchyLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserHierarchy");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserGroupUser");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserGroup");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UserDataPermissionLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UserDataPermission");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "UdfPermissionLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "UdfPermission");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TrueComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "TrueComponent");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "TemplateCategory");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TemplateBusinessLogicLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TeamUserLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "TeamUser");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TeamLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TaskIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TaskIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "TaskIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "TableMetadataLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "TableMetadata");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "SubModuleLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "SubModule");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "StartEventComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ServiceIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ServiceIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ServiceIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "RecTaskVersion");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "RecTask");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "RecruitmentPayElement");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ProjectEmailSetup");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ProcessDesignVariableLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ProcessDesignVariable");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ProcessDesignResultLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ProcessDesignResult");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ProcessDesignLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ProcessDesignComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ProcessDesign");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Portal");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "PermissionLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "PayrollTransaction");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "PayrollRun");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "PayrollBatch");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "PageTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "PageTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "PageNoteLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "PageNote");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "PageMemberLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "PageMemberGroupLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "PageIndexLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "PageIndexColumnLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "PageIndexColumn");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "PageIndex");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskTimeEntryLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskTimeEntry");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskSharedLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskShared");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskSequenceLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskSequence");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskPrecedenceLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskPrecedence");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTaskComment");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsTaskAttachmentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServiceStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServiceSharedLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsServiceShared");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServiceSequenceLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsServiceSequence");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServicePrecedenceLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsServicePrecedence");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsServiceComment");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNoteStatusTrackLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNoteStatusTrack");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNoteSharedLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNoteShared");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNoteSequenceLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNoteSequence");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNotePrecedenceLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNotePrecedence");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsNoteCommentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNoteComment");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsGroupUserGroup");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsGroupTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsGroup");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsCategoryLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NtsCategory");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NotificationTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NotificationTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NoteIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NoteIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "NoteIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ModuleLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "MemberLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "MemberGroupLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ManpowerSummaryComment");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "LOVLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "LOV");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ListOfValue");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobDescriptionCriteria");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobDescription");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobCriteriaTrack");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobCriteria");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobAdvertisementTrack");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "HiringManagerOrganization");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "HiringManager");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "HierarchyMasterLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "HierarchyMaster");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "HeadOfDepartmentOrganization");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "HeadOfDepartment");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "FormIndexPageTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "FormIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "FormIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "FormIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "FileLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "File");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "FalseComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "FalseComponent");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ExecutionScriptComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ExecutionScriptComponent");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "EmailSetting");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "EmailLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "EmailComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "EditorTypeLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "EditorLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "DecisionScriptComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "DecisionScriptComponent");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "DataIntegration");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ContextVariableLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ContextVariable");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ComponentResult");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ComponentParentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ComponentParent");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Component");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "CompleteEventComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "ColumnMetadataLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateSalaryDetail");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateReferences");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateProject");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateLanguageProficiency");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceBySector");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceByOther");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceByNature");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceByJob");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperienceByCountry");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateExperience");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateEvaluation");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateEducational");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateDrivingLicense");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "CandidateComputerProficiency");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessSection");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRuleNode");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRuleModel");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRuleGroup");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRuleConnector");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessRule");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "BusinessLogicComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "BusinessExecutionComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessData");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BusinessArea");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BreResult");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BreMasterTableMetadata");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "BreMasterColumnMetadata");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "AppointmentApprovalRequest");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationStatus");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationStateTrack");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationStateComment");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationState");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationSalaryDetail");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationReferences");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationProject");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationLanguageProficiency");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationJobCriteria");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperienceBySector");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperienceByOther");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperienceByJob");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperienceByCountry");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationExperience");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationeExperienceByNature");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationEducational");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationDrivingLicense");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationComputerProficiency");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "ApplicationBeneficiary");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "rec",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "AdhocTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "public",
                table: "AdhocTaskComponent");
        }
    }
}
