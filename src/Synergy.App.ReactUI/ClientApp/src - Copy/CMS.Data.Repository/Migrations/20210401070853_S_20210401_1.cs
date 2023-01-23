using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210401_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_NtsService_ParentServiceId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_NtsTask_ParentTaskId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_ParentServiceId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_ParentTaskId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ParentServiceId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.RenameColumn(
                name: "ParentTaskId",
                schema: "public",
                table: "NtsTask",
                newName: "ParentId");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundFileId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BannerFileId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundFileId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BannerFileId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundFileId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BannerFileId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ParentType",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundFileId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BannerFileId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "NtsNotePrecedence",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PredecessorId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PredecessorType = table.Column<int>(type: "integer", nullable: false),
                    PrecedenceRelationshipType = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_NtsNotePrecedence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsNotePrecedence_NtsNote_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "public",
                        principalTable: "NtsNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NtsServicePrecedence",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PredecessorId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PredecessorType = table.Column<int>(type: "integer", nullable: false),
                    PrecedenceRelationshipType = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_NtsServicePrecedence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsServicePrecedence_NtsService_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "public",
                        principalTable: "NtsService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NtsTaskPrecedence",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TaskId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PredecessorId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PredecessorType = table.Column<int>(type: "integer", nullable: false),
                    PrecedenceRelationshipType = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_NtsTaskPrecedence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsTaskPrecedence_NtsTask_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "public",
                        principalTable: "NtsTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NtsNotePrecedence_NoteId",
                schema: "public",
                table: "NtsNotePrecedence",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServicePrecedence_ServiceId",
                schema: "public",
                table: "NtsServicePrecedence",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskPrecedence_TaskId",
                schema: "public",
                table: "NtsTaskPrecedence",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NtsNotePrecedence",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NtsServicePrecedence",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NtsTaskPrecedence",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "BackgroundFileId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "BannerFileId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "BackgroundFileId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "BannerFileId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "BackgroundFileId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "BannerFileId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "ParentType",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "BackgroundFileId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "BannerFileId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                schema: "public",
                table: "NtsTask",
                newName: "ParentTaskId");

            migrationBuilder.AddColumn<string>(
                name: "ParentServiceId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_ParentServiceId",
                schema: "public",
                table: "NtsTask",
                column: "ParentServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_ParentTaskId",
                schema: "public",
                table: "NtsTask",
                column: "ParentTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_NtsService_ParentServiceId",
                schema: "public",
                table: "NtsTask",
                column: "ParentServiceId",
                principalSchema: "public",
                principalTable: "NtsService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_NtsTask_ParentTaskId",
                schema: "public",
                table: "NtsTask",
                column: "ParentTaskId",
                principalSchema: "public",
                principalTable: "NtsTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
