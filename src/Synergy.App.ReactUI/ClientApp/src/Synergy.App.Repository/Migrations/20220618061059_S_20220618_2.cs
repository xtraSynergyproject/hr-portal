using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220618_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayColumnId",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayColumnId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayColumnId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayColumnId",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayColumnId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayColumnId",
                schema: "log",
                table: "FormTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayColumnId",
                schema: "public",
                table: "FormTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceId",
                schema: "public",
                table: "BusinessRuleModel",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "StepTaskAssigneeLogic",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExecutionLogicDisplay = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExecutionLogic = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SuccessResult = table.Column<bool>(type: "boolean", nullable: false),
                    AssignedToTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
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
                    table.PrimaryKey("PK_StepTaskAssigneeLogic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepTaskAssigneeLogic_HierarchyMaster_AssignedToHierarchyMa~",
                        column: x => x.AssignedToHierarchyMasterId,
                        principalSchema: "public",
                        principalTable: "HierarchyMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskAssigneeLogic_LOV_AssignedToTypeId",
                        column: x => x.AssignedToTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskAssigneeLogic_Team_AssignedToTeamId",
                        column: x => x.AssignedToTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskAssigneeLogic_User_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StepTaskAssigneeLogicLog",
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
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExecutionLogicDisplay = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExecutionLogic = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SuccessResult = table.Column<bool>(type: "boolean", nullable: false),
                    AssignedToTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterLevelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepTaskAssigneeLogicLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepTaskAssigneeLogicLog_HierarchyMaster_AssignedToHierarch~",
                        column: x => x.AssignedToHierarchyMasterId,
                        principalSchema: "public",
                        principalTable: "HierarchyMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskAssigneeLogicLog_LOV_AssignedToTypeId",
                        column: x => x.AssignedToTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskAssigneeLogicLog_Team_AssignedToTeamId",
                        column: x => x.AssignedToTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskAssigneeLogicLog_User_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StepTaskSkipLogic",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExecutionLogicDisplay = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExecutionLogic = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SuccessResult = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_StepTaskSkipLogic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StepTaskSkipLogicLog",
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
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExecutionLogicDisplay = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExecutionLogic = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SuccessResult = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepTaskSkipLogicLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplateLog_DisplayColumnId",
                schema: "log",
                table: "TaskTemplateLog",
                column: "DisplayColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplate_DisplayColumnId",
                schema: "public",
                table: "TaskTemplate",
                column: "DisplayColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_DisplayColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DisplayColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_DisplayColumnId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DisplayColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTemplateLog_DisplayColumnId",
                schema: "log",
                table: "NoteTemplateLog",
                column: "DisplayColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTemplate_DisplayColumnId",
                schema: "public",
                table: "NoteTemplate",
                column: "DisplayColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FormTemplateLog_DisplayColumnId",
                schema: "log",
                table: "FormTemplateLog",
                column: "DisplayColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FormTemplate_DisplayColumnId",
                schema: "public",
                table: "FormTemplate",
                column: "DisplayColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskAssigneeLogic_AssignedToHierarchyMasterId",
                schema: "public",
                table: "StepTaskAssigneeLogic",
                column: "AssignedToHierarchyMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskAssigneeLogic_AssignedToTeamId",
                schema: "public",
                table: "StepTaskAssigneeLogic",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskAssigneeLogic_AssignedToTypeId",
                schema: "public",
                table: "StepTaskAssigneeLogic",
                column: "AssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskAssigneeLogic_AssignedToUserId",
                schema: "public",
                table: "StepTaskAssigneeLogic",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskAssigneeLogicLog_AssignedToHierarchyMasterId",
                schema: "log",
                table: "StepTaskAssigneeLogicLog",
                column: "AssignedToHierarchyMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskAssigneeLogicLog_AssignedToTeamId",
                schema: "log",
                table: "StepTaskAssigneeLogicLog",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskAssigneeLogicLog_AssignedToTypeId",
                schema: "log",
                table: "StepTaskAssigneeLogicLog",
                column: "AssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskAssigneeLogicLog_AssignedToUserId",
                schema: "log",
                table: "StepTaskAssigneeLogicLog",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormTemplate_ColumnMetadata_DisplayColumnId",
                schema: "public",
                table: "FormTemplate",
                column: "DisplayColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormTemplateLog_ColumnMetadata_DisplayColumnId",
                schema: "log",
                table: "FormTemplateLog",
                column: "DisplayColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTemplate_ColumnMetadata_DisplayColumnId",
                schema: "public",
                table: "NoteTemplate",
                column: "DisplayColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTemplateLog_ColumnMetadata_DisplayColumnId",
                schema: "log",
                table: "NoteTemplateLog",
                column: "DisplayColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_ColumnMetadata_DisplayColumnId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DisplayColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_ColumnMetadata_DisplayColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DisplayColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplate_ColumnMetadata_DisplayColumnId",
                schema: "public",
                table: "TaskTemplate",
                column: "DisplayColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplateLog_ColumnMetadata_DisplayColumnId",
                schema: "log",
                table: "TaskTemplateLog",
                column: "DisplayColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormTemplate_ColumnMetadata_DisplayColumnId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FormTemplateLog_ColumnMetadata_DisplayColumnId",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTemplate_ColumnMetadata_DisplayColumnId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTemplateLog_ColumnMetadata_DisplayColumnId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_ColumnMetadata_DisplayColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_ColumnMetadata_DisplayColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplate_ColumnMetadata_DisplayColumnId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplateLog_ColumnMetadata_DisplayColumnId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropTable(
                name: "StepTaskAssigneeLogic",
                schema: "public");

            migrationBuilder.DropTable(
                name: "StepTaskAssigneeLogicLog",
                schema: "log");

            migrationBuilder.DropTable(
                name: "StepTaskSkipLogic",
                schema: "public");

            migrationBuilder.DropTable(
                name: "StepTaskSkipLogicLog",
                schema: "log");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplateLog_DisplayColumnId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplate_DisplayColumnId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_DisplayColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_DisplayColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropIndex(
                name: "IX_NoteTemplateLog_DisplayColumnId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_NoteTemplate_DisplayColumnId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FormTemplateLog_DisplayColumnId",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_FormTemplate_DisplayColumnId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropColumn(
                name: "DisplayColumnId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "DisplayColumnId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DisplayColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DisplayColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DisplayColumnId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "DisplayColumnId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "DisplayColumnId",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropColumn(
                name: "DisplayColumnId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                schema: "public",
                table: "BusinessRuleModel");
        }
    }
}
