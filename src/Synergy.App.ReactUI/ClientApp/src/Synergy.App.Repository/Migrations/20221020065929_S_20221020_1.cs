using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221020_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RuntimeWorkflowButtonText",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "RuntimeWorkflowMandatory",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RuntimeWorkflowButtonText",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "RuntimeWorkflowMandatory",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RuntimeWorkflowButtonText",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "RuntimeWorkflowMandatory",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RuntimeWorkflowButtonText",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "RuntimeWorkflowMandatory",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RuntimeWorkflowDataId",
                schema: "log",
                table: "NtsTaskLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RuntimeWorkflowDataId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskLog_RuntimeWorkflowDataId",
                schema: "log",
                table: "NtsTaskLog",
                column: "RuntimeWorkflowDataId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_RuntimeWorkflowDataId",
                schema: "public",
                table: "NtsTask",
                column: "RuntimeWorkflowDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_RuntimeWorkflowData_RuntimeWorkflowDataId",
                schema: "public",
                table: "NtsTask",
                column: "RuntimeWorkflowDataId",
                principalSchema: "public",
                principalTable: "RuntimeWorkflowData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskLog_RuntimeWorkflowData_RuntimeWorkflowDataId",
                schema: "log",
                table: "NtsTaskLog",
                column: "RuntimeWorkflowDataId",
                principalSchema: "public",
                principalTable: "RuntimeWorkflowData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_RuntimeWorkflowData_RuntimeWorkflowDataId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskLog_RuntimeWorkflowData_RuntimeWorkflowDataId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskLog_RuntimeWorkflowDataId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_RuntimeWorkflowDataId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowButtonText",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowMandatory",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowButtonText",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowMandatory",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowButtonText",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowMandatory",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowButtonText",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowMandatory",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowDataId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowDataId",
                schema: "public",
                table: "NtsTask");
        }
    }
}
