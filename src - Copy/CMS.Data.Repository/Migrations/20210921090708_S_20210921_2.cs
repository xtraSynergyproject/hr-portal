using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210921_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangingNextTaskAssigneeTitle",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableChangingNextTaskAssignee",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableReturnTask",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReturnTaskButtonText",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnTaskTitle",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ChangingNextTaskAssigneeTitle",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableChangingNextTaskAssignee",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableReturnTask",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReturnTaskButtonText",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnTaskTitle",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsReopened",
                schema: "log",
                table: "NtsTaskLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                schema: "log",
                table: "NtsTaskLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NextTaskAssignedToHierarchyMasterId",
                schema: "log",
                table: "NtsTaskLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "NextTaskAssignedToHierarchyMasterLevelId",
                schema: "log",
                table: "NtsTaskLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextTaskAssignedToTeamId",
                schema: "log",
                table: "NtsTaskLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextTaskAssignedToTypeId",
                schema: "log",
                table: "NtsTaskLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextTaskAssignedToUserId",
                schema: "log",
                table: "NtsTaskLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsReopened",
                schema: "public",
                table: "NtsTask",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                schema: "public",
                table: "NtsTask",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NextTaskAssignedToHierarchyMasterId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "NextTaskAssignedToHierarchyMasterLevelId",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextTaskAssignedToTeamId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextTaskAssignedToTypeId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextTaskAssignedToUserId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_NextTaskAssignedToTeamId",
                schema: "log",
                table: "NtsTaskLog",
                column: "NextTaskAssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_NextTaskAssignedToTypeId",
                schema: "log",
                table: "NtsTaskLog",
                column: "NextTaskAssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_NextTaskAssignedToUserId",
                schema: "log",
                table: "NtsTaskLog",
                column: "NextTaskAssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_NextTaskAssignedToTeamId",
                schema: "public",
                table: "NtsTask",
                column: "NextTaskAssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_NextTaskAssignedToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "NextTaskAssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_NextTaskAssignedToUserId",
                schema: "public",
                table: "NtsTask",
                column: "NextTaskAssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_NextTaskAssignedToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "NextTaskAssignedToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_Team_NextTaskAssignedToTeamId",
                schema: "public",
                table: "NtsTask",
                column: "NextTaskAssignedToTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_User_NextTaskAssignedToUserId",
                schema: "public",
                table: "NtsTask",
                column: "NextTaskAssignedToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskLog_LOV_NextTaskAssignedToTypeId",
                schema: "log",
                table: "NtsTaskLog",
                column: "NextTaskAssignedToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskLog_Team_NextTaskAssignedToTeamId",
                schema: "log",
                table: "NtsTaskLog",
                column: "NextTaskAssignedToTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskLog_User_NextTaskAssignedToUserId",
                schema: "log",
                table: "NtsTaskLog",
                column: "NextTaskAssignedToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_NextTaskAssignedToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_Team_NextTaskAssignedToTeamId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_User_NextTaskAssignedToUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskLog_LOV_NextTaskAssignedToTypeId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskLog_Team_NextTaskAssignedToTeamId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskLog_User_NextTaskAssignedToUserId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskLog_NextTaskAssignedToTeamId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskLog_NextTaskAssignedToTypeId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskLog_NextTaskAssignedToUserId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_NextTaskAssignedToTeamId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_NextTaskAssignedToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_NextTaskAssignedToUserId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ChangingNextTaskAssigneeTitle",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "EnableChangingNextTaskAssignee",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "EnableReturnTask",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "ReturnTaskButtonText",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "ReturnTaskTitle",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "ChangingNextTaskAssigneeTitle",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "EnableChangingNextTaskAssignee",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "EnableReturnTask",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "ReturnTaskButtonText",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "ReturnTaskTitle",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "IsReopened",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "IsReturned",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToHierarchyMasterId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToHierarchyMasterLevelId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToTeamId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToTypeId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToUserId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "IsReopened",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "IsReturned",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToHierarchyMasterId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToHierarchyMasterLevelId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToTeamId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "NextTaskAssignedToUserId",
                schema: "public",
                table: "NtsTask");
        }
    }
}
