using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210518_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "log",
                table: "ModuleLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "public",
                table: "Module",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateLog_ModuleId",
                schema: "log",
                table: "TemplateLog",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_ModuleId",
                schema: "public",
                table: "Template",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_Module_ModuleId",
                schema: "public",
                table: "Template",
                column: "ModuleId",
                principalSchema: "public",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateLog_Module_ModuleId",
                schema: "log",
                table: "TemplateLog",
                column: "ModuleId",
                principalSchema: "public",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_Module_ModuleId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateLog_Module_ModuleId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TemplateLog_ModuleId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_Template_ModuleId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "log",
                table: "ModuleLog");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "public",
                table: "Module");
        }
    }
}
