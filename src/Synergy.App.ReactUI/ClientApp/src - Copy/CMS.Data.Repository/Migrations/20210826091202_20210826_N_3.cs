using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210826_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NtsNoteEvent",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NtsServiceEvent",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NtsTaskEvent",
                schema: "public");

            migrationBuilder.AddColumn<string>(
                name: "TaskEventId",
                schema: "log",
                table: "NtsTaskLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TaskEventId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServiceEventId",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ServiceEventId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NoteEventId",
                schema: "log",
                table: "NtsNoteLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NoteEventId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_TaskEventId",
                schema: "log",
                table: "NtsTaskLog",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TaskEventId",
                schema: "public",
                table: "NtsTask",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceLog_ServiceEventId",
                schema: "log",
                table: "NtsServiceLog",
                column: "ServiceEventId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_ServiceEventId",
                schema: "public",
                table: "NtsService",
                column: "ServiceEventId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteLog_NoteEventId",
                schema: "log",
                table: "NtsNoteLog",
                column: "NoteEventId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNote_NoteEventId",
                schema: "public",
                table: "NtsNote",
                column: "NoteEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNote_LOV_NoteEventId",
                schema: "public",
                table: "NtsNote",
                column: "NoteEventId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNoteLog_LOV_NoteEventId",
                schema: "log",
                table: "NtsNoteLog",
                column: "NoteEventId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_LOV_ServiceEventId",
                schema: "public",
                table: "NtsService",
                column: "ServiceEventId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsServiceLog_LOV_ServiceEventId",
                schema: "log",
                table: "NtsServiceLog",
                column: "ServiceEventId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_TaskEventId",
                schema: "public",
                table: "NtsTask",
                column: "TaskEventId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskLog_LOV_TaskEventId",
                schema: "log",
                table: "NtsTaskLog",
                column: "TaskEventId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsNote_LOV_NoteEventId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsNoteLog_LOV_NoteEventId",
                schema: "log",
                table: "NtsNoteLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_LOV_ServiceEventId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsServiceLog_LOV_ServiceEventId",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_TaskEventId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskLog_LOV_TaskEventId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskLog_TaskEventId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TaskEventId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsServiceLog_ServiceEventId",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_ServiceEventId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsNoteLog_NoteEventId",
                schema: "log",
                table: "NtsNoteLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsNote_NoteEventId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "TaskEventId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "TaskEventId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ServiceEventId",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "ServiceEventId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "NoteEventId",
                schema: "log",
                table: "NtsNoteLog");

            migrationBuilder.DropColumn(
                name: "NoteEventId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.CreateTable(
                name: "NtsNoteEvent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Access = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AppliesTo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AssignTo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataOperationEvent = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LegalEntityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteStatusCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PermissionType = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ReferenceTo = table.Column<long>(type: "bigint", nullable: true),
                    ReferenceType = table.Column<int>(type: "integer", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TagName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsNoteEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NtsServiceEvent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LegalEntityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SLA = table.Column<int>(type: "integer", nullable: true),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    ServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsServiceEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NtsTaskEvent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LegalEntityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SLA = table.Column<int>(type: "integer", nullable: true),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsTaskEvent", x => x.Id);
                });
        }
    }
}
