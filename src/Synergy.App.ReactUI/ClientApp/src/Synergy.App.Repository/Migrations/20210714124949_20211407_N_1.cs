using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20211407_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                schema: "log",
                table: "TemplateCategoryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                schema: "public",
                table: "TemplateCategory",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCategoryLog_ModuleId",
                schema: "log",
                table: "TemplateCategoryLog",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCategory_ModuleId",
                schema: "public",
                table: "TemplateCategory",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateCategory_Module_ModuleId",
                schema: "public",
                table: "TemplateCategory",
                column: "ModuleId",
                principalSchema: "public",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateCategoryLog_Module_ModuleId",
                schema: "log",
                table: "TemplateCategoryLog",
                column: "ModuleId",
                principalSchema: "public",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateCategory_Module_ModuleId",
                schema: "public",
                table: "TemplateCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateCategoryLog_Module_ModuleId",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropIndex(
                name: "IX_TemplateCategoryLog_ModuleId",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropIndex(
                name: "IX_TemplateCategory_ModuleId",
                schema: "public",
                table: "TemplateCategory");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "public",
                table: "TemplateCategory");
        }
    }
}
