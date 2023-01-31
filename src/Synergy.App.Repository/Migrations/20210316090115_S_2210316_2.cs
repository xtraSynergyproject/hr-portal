using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210316_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsNote_LOV_NoteActionStatusId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropTable(
                name: "NtsTaskVersion",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_NtsNote_NoteActionStatusId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "NoteActionStatusId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.RenameColumn(
                name: "SLA",
                schema: "public",
                table: "NtsTask",
                newName: "VersionByUserId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NtsTaskId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "VersionDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_NtsTaskId",
                schema: "public",
                table: "NtsTask",
                column: "NtsTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_NtsTask_NtsTaskId",
                schema: "public",
                table: "NtsTask",
                column: "NtsTaskId",
                principalSchema: "public",
                principalTable: "NtsTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_NtsTask_NtsTaskId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_NtsTaskId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "NtsTaskId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "VersionDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.RenameColumn(
                name: "VersionByUserId",
                schema: "public",
                table: "NtsTask",
                newName: "SLA");

            migrationBuilder.AddColumn<string>(
                name: "NoteActionStatusId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "NtsTaskVersion",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssgineeUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignToType = table.Column<int>(type: "integer", nullable: true),
                    AssigneeTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssigneeUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CancelReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompletedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DelegateReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DelegatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownDisplay5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DropdownValue5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsAssignedInTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: true),
                    IsTaskAutoComplete = table.Column<bool>(type: "boolean", nullable: false),
                    IsTaskAutoCompleteIfSameAssignee = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LockStatus = table.Column<int>(type: "integer", nullable: true),
                    OwnerUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PlanOrder = table.Column<long>(type: "bigint", nullable: true),
                    RatingComment = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReferenceMasterId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReferenceTypeCode = table.Column<int>(type: "integer", nullable: true),
                    ReferenceTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RejectedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReminderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReopenReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReopenedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RequestedUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReturnReason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReturnedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SLA = table.Column<TimeSpan>(type: "interval", nullable: true),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TaskNo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskStatus = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextDisplay5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TextValue5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsTaskVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsTaskVersion_TaskTemplate_TaskTemplateId",
                        column: x => x.TaskTemplateId,
                        principalSchema: "public",
                        principalTable: "TaskTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskVersion_Team_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskVersion_User_AssgineeUserId",
                        column: x => x.AssgineeUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskVersion_User_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsTaskVersion_User_RequestedUserId",
                        column: x => x.RequestedUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NtsNote_NoteActionStatusId",
                schema: "public",
                table: "NtsNote",
                column: "NoteActionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskVersion_AssgineeUserId",
                schema: "public",
                table: "NtsTaskVersion",
                column: "AssgineeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskVersion_OwnerUserId",
                schema: "public",
                table: "NtsTaskVersion",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskVersion_RequestedUserId",
                schema: "public",
                table: "NtsTaskVersion",
                column: "RequestedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskVersion_TaskTemplateId",
                schema: "public",
                table: "NtsTaskVersion",
                column: "TaskTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskVersion_TeamId",
                schema: "public",
                table: "NtsTaskVersion",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNote_LOV_NoteActionStatusId",
                schema: "public",
                table: "NtsNote",
                column: "NoteActionStatusId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
