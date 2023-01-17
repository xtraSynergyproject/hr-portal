using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210717_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemplateStepId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateStepId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateLog_TemplateStepId",
                schema: "log",
                table: "TemplateLog",
                column: "TemplateStepId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_TemplateStepId",
                schema: "public",
                table: "Template",
                column: "TemplateStepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_TemplateStage_TemplateStepId",
                schema: "public",
                table: "Template",
                column: "TemplateStepId",
                principalSchema: "public",
                principalTable: "TemplateStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateLog_TemplateStage_TemplateStepId",
                schema: "log",
                table: "TemplateLog",
                column: "TemplateStepId",
                principalSchema: "public",
                principalTable: "TemplateStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_TemplateStage_TemplateStepId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateLog_TemplateStage_TemplateStepId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TemplateLog_TemplateStepId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_Template_TemplateStepId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "TemplateStepId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "TemplateStepId",
                schema: "public",
                table: "Template");
        }
    }
}
