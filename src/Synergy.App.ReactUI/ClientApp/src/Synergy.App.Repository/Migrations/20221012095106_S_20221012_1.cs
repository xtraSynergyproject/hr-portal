using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221012_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableRuntimeWorkflow",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRuntimeComponent",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RuntimeAssigneeTeamListUrl",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RuntimeAssigneeUserListUrl",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableRuntimeWorkflow",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRuntimeComponent",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RuntimeAssigneeTeamListUrl",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RuntimeAssigneeUserListUrl",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableRuntimeWorkflow",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RuntimeAssigneeTeamListUrl",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RuntimeAssigneeUserListUrl",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableRuntimeWorkflow",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RuntimeAssigneeTeamListUrl",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RuntimeAssigneeUserListUrl",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "RuntimeWorkflow",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RuntimeWorkflowSourceTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SourceServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SourceTaskId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RuntimeWorkflowExecutionMode = table.Column<int>(type: "integer", nullable: false),
                    TriggeringStepTaskComponentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TriggeringTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TriggeringComponentId = table.Column<string>(type: "text", nullable: true)
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
                    LegalEntityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuntimeWorkflow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflow_Component_TriggeringComponentId",
                        column: x => x.TriggeringComponentId,
                        principalSchema: "public",
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflow_StepTaskComponent_TriggeringStepTaskCompone~",
                        column: x => x.TriggeringStepTaskComponentId,
                        principalSchema: "public",
                        principalTable: "StepTaskComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflow_Template_RuntimeWorkflowSourceTemplateId",
                        column: x => x.RuntimeWorkflowSourceTemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflow_Template_TriggeringTemplateId",
                        column: x => x.TriggeringTemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RuntimeWorkflowLog",
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
                    RuntimeWorkflowSourceTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SourceServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SourceTaskId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RuntimeWorkflowExecutionMode = table.Column<int>(type: "integer", nullable: false),
                    TriggeringStepTaskComponentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TriggeringTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TriggeringComponentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuntimeWorkflowLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowLog_Component_TriggeringComponentId",
                        column: x => x.TriggeringComponentId,
                        principalSchema: "public",
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowLog_StepTaskComponent_TriggeringStepTaskComp~",
                        column: x => x.TriggeringStepTaskComponentId,
                        principalSchema: "public",
                        principalTable: "StepTaskComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowLog_Template_RuntimeWorkflowSourceTemplateId",
                        column: x => x.RuntimeWorkflowSourceTemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowLog_Template_TriggeringTemplateId",
                        column: x => x.TriggeringTemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RuntimeWorkflowData",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RuntimeWorkflowId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TeamAssignmentType = table.Column<int>(type: "integer", nullable: false),
                    AssignedToHierarchyMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterLevelId = table.Column<int>(type: "integer", nullable: false),
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
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuntimeWorkflowData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowData_HierarchyMaster_AssignedToHierarchyMast~",
                        column: x => x.AssignedToHierarchyMasterId,
                        principalSchema: "public",
                        principalTable: "HierarchyMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowData_LOV_AssignedToTypeId",
                        column: x => x.AssignedToTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowData_RuntimeWorkflow_RuntimeWorkflowId",
                        column: x => x.RuntimeWorkflowId,
                        principalSchema: "public",
                        principalTable: "RuntimeWorkflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowData_Team_AssignedToTeamId",
                        column: x => x.AssignedToTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowData_User_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RuntimeWorkflowDataLog",
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
                    RuntimeWorkflowId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TeamAssignmentType = table.Column<int>(type: "integer", nullable: false),
                    AssignedToHierarchyMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterLevelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuntimeWorkflowDataLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowDataLog_HierarchyMaster_AssignedToHierarchyM~",
                        column: x => x.AssignedToHierarchyMasterId,
                        principalSchema: "public",
                        principalTable: "HierarchyMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowDataLog_LOV_AssignedToTypeId",
                        column: x => x.AssignedToTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowDataLog_RuntimeWorkflow_RuntimeWorkflowId",
                        column: x => x.RuntimeWorkflowId,
                        principalSchema: "public",
                        principalTable: "RuntimeWorkflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowDataLog_Team_AssignedToTeamId",
                        column: x => x.AssignedToTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuntimeWorkflowDataLog_User_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflow_RuntimeWorkflowSourceTemplateId",
                schema: "public",
                table: "RuntimeWorkflow",
                column: "RuntimeWorkflowSourceTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflow_TriggeringComponentId",
                schema: "public",
                table: "RuntimeWorkflow",
                column: "TriggeringComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflow_TriggeringStepTaskComponentId",
                schema: "public",
                table: "RuntimeWorkflow",
                column: "TriggeringStepTaskComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflow_TriggeringTemplateId",
                schema: "public",
                table: "RuntimeWorkflow",
                column: "TriggeringTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowData_AssignedToHierarchyMasterId",
                schema: "public",
                table: "RuntimeWorkflowData",
                column: "AssignedToHierarchyMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowData_AssignedToTeamId",
                schema: "public",
                table: "RuntimeWorkflowData",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowData_AssignedToTypeId",
                schema: "public",
                table: "RuntimeWorkflowData",
                column: "AssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowData_AssignedToUserId",
                schema: "public",
                table: "RuntimeWorkflowData",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowData_RuntimeWorkflowId",
                schema: "public",
                table: "RuntimeWorkflowData",
                column: "RuntimeWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowDataLog_AssignedToHierarchyMasterId",
                schema: "log",
                table: "RuntimeWorkflowDataLog",
                column: "AssignedToHierarchyMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowDataLog_AssignedToTeamId",
                schema: "log",
                table: "RuntimeWorkflowDataLog",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowDataLog_AssignedToTypeId",
                schema: "log",
                table: "RuntimeWorkflowDataLog",
                column: "AssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowDataLog_AssignedToUserId",
                schema: "log",
                table: "RuntimeWorkflowDataLog",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowDataLog_RuntimeWorkflowId",
                schema: "log",
                table: "RuntimeWorkflowDataLog",
                column: "RuntimeWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowLog_RuntimeWorkflowSourceTemplateId",
                schema: "log",
                table: "RuntimeWorkflowLog",
                column: "RuntimeWorkflowSourceTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowLog_TriggeringComponentId",
                schema: "log",
                table: "RuntimeWorkflowLog",
                column: "TriggeringComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowLog_TriggeringStepTaskComponentId",
                schema: "log",
                table: "RuntimeWorkflowLog",
                column: "TriggeringStepTaskComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RuntimeWorkflowLog_TriggeringTemplateId",
                schema: "log",
                table: "RuntimeWorkflowLog",
                column: "TriggeringTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuntimeWorkflowData",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RuntimeWorkflowDataLog",
                schema: "log");

            migrationBuilder.DropTable(
                name: "RuntimeWorkflowLog",
                schema: "log");

            migrationBuilder.DropTable(
                name: "RuntimeWorkflow",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "EnableRuntimeWorkflow",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "IsRuntimeComponent",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "RuntimeAssigneeTeamListUrl",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "RuntimeAssigneeUserListUrl",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "EnableRuntimeWorkflow",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "IsRuntimeComponent",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "RuntimeAssigneeTeamListUrl",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "RuntimeAssigneeUserListUrl",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "EnableRuntimeWorkflow",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "RuntimeAssigneeTeamListUrl",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "RuntimeAssigneeUserListUrl",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableRuntimeWorkflow",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "RuntimeAssigneeTeamListUrl",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "RuntimeAssigneeUserListUrl",
                schema: "public",
                table: "ServiceTemplate");
        }
    }
}
