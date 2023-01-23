using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210506_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "DataIntegration",
                schema: "bre",
                newName: "DataIntegration",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BusinessSection",
                schema: "bre",
                newName: "BusinessSection",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BusinessRuleNode",
                schema: "bre",
                newName: "BusinessRuleNode",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BusinessRuleModel",
                schema: "bre",
                newName: "BusinessRuleModel",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BusinessRuleGroup",
                schema: "bre",
                newName: "BusinessRuleGroup",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BusinessRuleConnector",
                schema: "bre",
                newName: "BusinessRuleConnector",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BusinessRule",
                schema: "bre",
                newName: "BusinessRule",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BusinessData",
                schema: "bre",
                newName: "BusinessData",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BusinessArea",
                schema: "bre",
                newName: "BusinessArea",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BreResult",
                schema: "bre",
                newName: "BreResult",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BreMetadata",
                schema: "bre",
                newName: "BreMetadata",
                newSchema: "public");

            migrationBuilder.AddColumn<string>(
                name: "ActionId",
                schema: "public",
                table: "BusinessRule",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                schema: "public",
                table: "BusinessRule",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRule_ActionId",
                schema: "public",
                table: "BusinessRule",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRule_TemplateId",
                schema: "public",
                table: "BusinessRule",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRule_LOV_ActionId",
                schema: "public",
                table: "BusinessRule",
                column: "ActionId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRule_Template_TemplateId",
                schema: "public",
                table: "BusinessRule",
                column: "TemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRule_LOV_ActionId",
                schema: "public",
                table: "BusinessRule");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRule_Template_TemplateId",
                schema: "public",
                table: "BusinessRule");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRule_ActionId",
                schema: "public",
                table: "BusinessRule");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRule_TemplateId",
                schema: "public",
                table: "BusinessRule");

            migrationBuilder.DropColumn(
                name: "ActionId",
                schema: "public",
                table: "BusinessRule");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "public",
                table: "BusinessRule");

            migrationBuilder.EnsureSchema(
                name: "bre");

            migrationBuilder.RenameTable(
                name: "DataIntegration",
                schema: "public",
                newName: "DataIntegration",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BusinessSection",
                schema: "public",
                newName: "BusinessSection",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BusinessRuleNode",
                schema: "public",
                newName: "BusinessRuleNode",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BusinessRuleModel",
                schema: "public",
                newName: "BusinessRuleModel",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BusinessRuleGroup",
                schema: "public",
                newName: "BusinessRuleGroup",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BusinessRuleConnector",
                schema: "public",
                newName: "BusinessRuleConnector",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BusinessRule",
                schema: "public",
                newName: "BusinessRule",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BusinessData",
                schema: "public",
                newName: "BusinessData",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BusinessArea",
                schema: "public",
                newName: "BusinessArea",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BreResult",
                schema: "public",
                newName: "BreResult",
                newSchema: "bre");

            migrationBuilder.RenameTable(
                name: "BreMetadata",
                schema: "public",
                newName: "BreMetadata",
                newSchema: "bre");
        }
    }
}
