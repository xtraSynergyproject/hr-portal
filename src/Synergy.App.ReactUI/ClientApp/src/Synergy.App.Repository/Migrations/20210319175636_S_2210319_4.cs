using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210319_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_NtsTask_NtsTaskId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.RenameColumn(
                name: "TaskDescription",
                schema: "public",
                table: "StepTaskComponent",
                newName: "TaskTemplateId");

            migrationBuilder.RenameColumn(
                name: "StepTaskType",
                schema: "public",
                table: "StepTaskComponent",
                newName: "AssignedToType");

            migrationBuilder.RenameColumn(
                name: "RequestedByUserId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "ReturnStepTaskId");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "NotificationSubject");

            migrationBuilder.RenameColumn(
                name: "NtsTaskId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "AssignedToHierarchyMasterId");

            migrationBuilder.RenameColumn(
                name: "AssigneeType",
                schema: "public",
                table: "StepTaskComponent",
                newName: "AssignedToHierarchyMasterLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_StepTaskComponent_NtsTaskId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "IX_StepTaskComponent_AssignedToHierarchyMasterId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NotificationSubject",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "DecisionScriptComponent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Script = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ComponentId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_DecisionScriptComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionScriptComponent_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "public",
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionScriptComponent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Script = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ComponentId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_ExecutionScriptComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionScriptComponent_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "public",
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FalseComponent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ComponentId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_FalseComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FalseComponent_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "public",
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrueComponent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ComponentId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_TrueComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrueComponent_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "public",
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_AssignedToTeamId",
                schema: "public",
                table: "StepTaskComponent",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_AssignedToUserId",
                schema: "public",
                table: "StepTaskComponent",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_ReturnStepTaskId",
                schema: "public",
                table: "StepTaskComponent",
                column: "ReturnStepTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_TaskTemplateId",
                schema: "public",
                table: "StepTaskComponent",
                column: "TaskTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionScriptComponent_ComponentId",
                schema: "public",
                table: "DecisionScriptComponent",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionScriptComponent_ComponentId",
                schema: "public",
                table: "ExecutionScriptComponent",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FalseComponent_ComponentId",
                schema: "public",
                table: "FalseComponent",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueComponent_ComponentId",
                schema: "public",
                table: "TrueComponent",
                column: "ComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_HierarchyMaster_AssignedToHierarchyMaster~",
                schema: "public",
                table: "StepTaskComponent",
                column: "AssignedToHierarchyMasterId",
                principalSchema: "public",
                principalTable: "HierarchyMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_TaskTemplate_ReturnStepTaskId",
                schema: "public",
                table: "StepTaskComponent",
                column: "ReturnStepTaskId",
                principalSchema: "public",
                principalTable: "TaskTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_TaskTemplate_TaskTemplateId",
                schema: "public",
                table: "StepTaskComponent",
                column: "TaskTemplateId",
                principalSchema: "public",
                principalTable: "TaskTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_Team_AssignedToTeamId",
                schema: "public",
                table: "StepTaskComponent",
                column: "AssignedToTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_User_AssignedToUserId",
                schema: "public",
                table: "StepTaskComponent",
                column: "AssignedToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_HierarchyMaster_AssignedToHierarchyMaster~",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_TaskTemplate_ReturnStepTaskId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_TaskTemplate_TaskTemplateId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_Team_AssignedToTeamId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_User_AssignedToUserId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropTable(
                name: "DecisionScriptComponent",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExecutionScriptComponent",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FalseComponent",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrueComponent",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_AssignedToTeamId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_AssignedToUserId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_ReturnStepTaskId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_TaskTemplateId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "NotificationSubject",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.RenameColumn(
                name: "TaskTemplateId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "TaskDescription");

            migrationBuilder.RenameColumn(
                name: "ReturnStepTaskId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "RequestedByUserId");

            migrationBuilder.RenameColumn(
                name: "NotificationSubject",
                schema: "public",
                table: "StepTaskComponent",
                newName: "OwnerUserId");

            migrationBuilder.RenameColumn(
                name: "AssignedToType",
                schema: "public",
                table: "StepTaskComponent",
                newName: "StepTaskType");

            migrationBuilder.RenameColumn(
                name: "AssignedToHierarchyMasterLevelId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "AssigneeType");

            migrationBuilder.RenameColumn(
                name: "AssignedToHierarchyMasterId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "NtsTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_StepTaskComponent_AssignedToHierarchyMasterId",
                schema: "public",
                table: "StepTaskComponent",
                newName: "IX_StepTaskComponent_NtsTaskId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_NtsTask_NtsTaskId",
                schema: "public",
                table: "StepTaskComponent",
                column: "NtsTaskId",
                principalSchema: "public",
                principalTable: "NtsTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
