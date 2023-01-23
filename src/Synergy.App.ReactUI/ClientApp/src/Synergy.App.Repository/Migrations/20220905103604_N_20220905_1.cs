using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class N_20220905_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                schema: "log",
                table: "UserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentSubject",
                schema: "log",
                table: "NtsTaskCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentSubject",
                schema: "public",
                table: "NtsTaskComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentSubject",
                schema: "log",
                table: "NtsServiceCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentSubject",
                schema: "public",
                table: "NtsServiceComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "StepTaskEscalation",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepTaskComponentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentStepTaskEscalationId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepTaskEscalationType = table.Column<int>(type: "integer", nullable: false),
                    AssignedToTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterLevelId = table.Column<int>(type: "integer", nullable: true),
                    NewPriorityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TriggerDaysAfterOverDue = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_StepTaskEscalation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalation_LOV_AssignedToTypeId",
                        column: x => x.AssignedToTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalation_LOV_NewPriorityId",
                        column: x => x.NewPriorityId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalation_NotificationTemplate_NotificationTemplat~",
                        column: x => x.NotificationTemplateId,
                        principalSchema: "public",
                        principalTable: "NotificationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalation_StepTaskComponent_StepTaskComponentId",
                        column: x => x.StepTaskComponentId,
                        principalSchema: "public",
                        principalTable: "StepTaskComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalation_StepTaskEscalation_ParentStepTaskEscalat~",
                        column: x => x.ParentStepTaskEscalationId,
                        principalSchema: "public",
                        principalTable: "StepTaskEscalation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalation_Team_AssignedToTeamId",
                        column: x => x.AssignedToTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalation_User_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StepTaskEscalationLog",
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
                    StepTaskComponentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentStepTaskEscalationId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StepTaskEscalationType = table.Column<int>(type: "integer", nullable: false),
                    AssignedToTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignedToHierarchyMasterLevelId = table.Column<int>(type: "integer", nullable: true),
                    NewPriorityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TriggerDaysAfterOverDue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepTaskEscalationLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalationLog_LOV_AssignedToTypeId",
                        column: x => x.AssignedToTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalationLog_LOV_NewPriorityId",
                        column: x => x.NewPriorityId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalationLog_NotificationTemplate_NotificationTemp~",
                        column: x => x.NotificationTemplateId,
                        principalSchema: "public",
                        principalTable: "NotificationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalationLog_StepTaskComponent_StepTaskComponentId",
                        column: x => x.StepTaskComponentId,
                        principalSchema: "public",
                        principalTable: "StepTaskComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalationLog_StepTaskEscalation_ParentStepTaskEsca~",
                        column: x => x.ParentStepTaskEscalationId,
                        principalSchema: "public",
                        principalTable: "StepTaskEscalation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalationLog_Team_AssignedToTeamId",
                        column: x => x.AssignedToTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepTaskEscalationLog_User_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalation_AssignedToTeamId",
                schema: "public",
                table: "StepTaskEscalation",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalation_AssignedToTypeId",
                schema: "public",
                table: "StepTaskEscalation",
                column: "AssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalation_AssignedToUserId",
                schema: "public",
                table: "StepTaskEscalation",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalation_NewPriorityId",
                schema: "public",
                table: "StepTaskEscalation",
                column: "NewPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalation_NotificationTemplateId",
                schema: "public",
                table: "StepTaskEscalation",
                column: "NotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalation_ParentStepTaskEscalationId",
                schema: "public",
                table: "StepTaskEscalation",
                column: "ParentStepTaskEscalationId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalation_StepTaskComponentId",
                schema: "public",
                table: "StepTaskEscalation",
                column: "StepTaskComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationLog_AssignedToTeamId",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationLog_AssignedToTypeId",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "AssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationLog_AssignedToUserId",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationLog_NewPriorityId",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "NewPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationLog_NotificationTemplateId",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "NotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationLog_ParentStepTaskEscalationId",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "ParentStepTaskEscalationId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationLog_StepTaskComponentId",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "StepTaskComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StepTaskEscalationLog",
                schema: "log");

            migrationBuilder.DropTable(
                name: "StepTaskEscalation",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CommentSubject",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropColumn(
                name: "CommentSubject",
                schema: "public",
                table: "NtsTaskComment");

            migrationBuilder.DropColumn(
                name: "CommentSubject",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropColumn(
                name: "CommentSubject",
                schema: "public",
                table: "NtsServiceComment");
        }
    }
}
