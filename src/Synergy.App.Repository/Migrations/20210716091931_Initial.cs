using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemplateStageId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateStageId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "TemplateStage",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentStageId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_TemplateStage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateLog_TemplateStageId",
                schema: "log",
                table: "TemplateLog",
                column: "TemplateStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_TemplateStageId",
                schema: "public",
                table: "Template",
                column: "TemplateStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_TemplateStage_TemplateStageId",
                schema: "public",
                table: "Template",
                column: "TemplateStageId",
                principalSchema: "public",
                principalTable: "TemplateStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateLog_TemplateStage_TemplateStageId",
                schema: "log",
                table: "TemplateLog",
                column: "TemplateStageId",
                principalSchema: "public",
                principalTable: "TemplateStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_TemplateStage_TemplateStageId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateLog_TemplateStage_TemplateStageId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropTable(
                name: "TemplateStage",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_TemplateLog_TemplateStageId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_Template_TemplateStageId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "TemplateStageId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "TemplateStageId",
                schema: "public",
                table: "Template");
        }
    }
}
