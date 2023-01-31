using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210511_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponentLog_UdfTemplateId",
                schema: "log",
                table: "StepTaskComponentLog",
                column: "UdfTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_UdfTemplateId",
                schema: "public",
                table: "StepTaskComponent",
                column: "UdfTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_Template_UdfTemplateId",
                schema: "public",
                table: "StepTaskComponent",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponentLog_Template_UdfTemplateId",
                schema: "log",
                table: "StepTaskComponentLog",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_Template_UdfTemplateId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponentLog_Template_UdfTemplateId",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponentLog_UdfTemplateId",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_UdfTemplateId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "public",
                table: "StepTaskComponent");
        }
    }
}
