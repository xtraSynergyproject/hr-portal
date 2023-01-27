using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210430_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteNotificationTemplate",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ServiceNotificationTemplate",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TaskNotificationTemplate",
                schema: "public");

            migrationBuilder.RenameColumn(
                name: "ReferenceTypeName",
                schema: "public",
                table: "Notification",
                newName: "ReferenceType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReferenceType",
                schema: "public",
                table: "Notification",
                newName: "ReferenceTypeName");

            migrationBuilder.CreateTable(
                name: "NoteNotificationTemplate",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Body = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CopyFromTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationActionId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationTo = table.Column<int>(type: "integer", nullable: false),
                    NotifyByEmail = table.Column<bool>(type: "boolean", nullable: false),
                    NotifyBySms = table.Column<bool>(type: "boolean", nullable: false),
                    NtsType = table.Column<int>(type: "integer", nullable: true),
                    ParentNoteNotificationTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SendAlways = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    SmsText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TargetUserTypes = table.Column<string[]>(type: "text[]", nullable: true),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteNotificationTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteNotificationTemplate_NoteNotificationTemplate_ParentNot~",
                        column: x => x.ParentNoteNotificationTemplateId,
                        principalSchema: "public",
                        principalTable: "NoteNotificationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NoteNotificationTemplate_NoteTemplate_NoteTemplateId",
                        column: x => x.NoteTemplateId,
                        principalSchema: "public",
                        principalTable: "NoteTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceNotificationTemplate",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Body = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CopyFromTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationActionId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationTo = table.Column<int>(type: "integer", nullable: false),
                    NotifyByEmail = table.Column<bool>(type: "boolean", nullable: false),
                    NotifyBySms = table.Column<bool>(type: "boolean", nullable: false),
                    NtsType = table.Column<int>(type: "integer", nullable: true),
                    ParentServiceNotificationTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SendAlways = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    ServiceTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SmsText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TargetUserTypes = table.Column<string[]>(type: "text[]", nullable: true),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceNotificationTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceNotificationTemplate_ServiceNotificationTemplate_Par~",
                        column: x => x.ParentServiceNotificationTemplateId,
                        principalSchema: "public",
                        principalTable: "ServiceNotificationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceNotificationTemplate_ServiceTemplate_ServiceTemplate~",
                        column: x => x.ServiceTemplateId,
                        principalSchema: "public",
                        principalTable: "ServiceTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskNotificationTemplate",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Body = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CopyFromTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationActionId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NotificationTo = table.Column<int>(type: "integer", nullable: false),
                    NotifyByEmail = table.Column<bool>(type: "boolean", nullable: false),
                    NotifyBySms = table.Column<bool>(type: "boolean", nullable: false),
                    NtsType = table.Column<int>(type: "integer", nullable: true),
                    ParentTaskNotificationTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SendAlways = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    SmsText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TargetUserTypes = table.Column<string[]>(type: "text[]", nullable: true),
                    TaskTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskNotificationTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskNotificationTemplate_TaskNotificationTemplate_ParentTas~",
                        column: x => x.ParentTaskNotificationTemplateId,
                        principalSchema: "public",
                        principalTable: "TaskNotificationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskNotificationTemplate_TaskTemplate_TaskTemplateId",
                        column: x => x.TaskTemplateId,
                        principalSchema: "public",
                        principalTable: "TaskTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteNotificationTemplate_NoteTemplateId",
                schema: "public",
                table: "NoteNotificationTemplate",
                column: "NoteTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteNotificationTemplate_ParentNoteNotificationTemplateId",
                schema: "public",
                table: "NoteNotificationTemplate",
                column: "ParentNoteNotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceNotificationTemplate_ParentServiceNotificationTempla~",
                schema: "public",
                table: "ServiceNotificationTemplate",
                column: "ParentServiceNotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceNotificationTemplate_ServiceTemplateId",
                schema: "public",
                table: "ServiceNotificationTemplate",
                column: "ServiceTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskNotificationTemplate_ParentTaskNotificationTemplateId",
                schema: "public",
                table: "TaskNotificationTemplate",
                column: "ParentTaskNotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskNotificationTemplate_TaskTemplateId",
                schema: "public",
                table: "TaskNotificationTemplate",
                column: "TaskTemplateId");
        }
    }
}
