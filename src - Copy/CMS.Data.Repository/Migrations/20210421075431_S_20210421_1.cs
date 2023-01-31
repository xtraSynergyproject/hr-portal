using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210421_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskTemplateType",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TemplateCreateType",
                schema: "public",
                table: "StepTaskComponent",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "TaskType",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_TemplateId",
                schema: "public",
                table: "StepTaskComponent",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_Template_TemplateId",
                schema: "public",
                table: "StepTaskComponent",
                column: "TemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_Template_TemplateId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_TemplateId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "TaskTemplateType",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TemplateCreateType",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "TaskType",
                schema: "public",
                table: "NtsTask");
        }
    }
}
