using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210928_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableDynamicStepTaskSelection",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableDynamicStepTaskSelection",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableDynamicStepTaskSelection",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableDynamicStepTaskSelection",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NextStepTaskComponentId",
                schema: "log",
                table: "NtsTaskLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextStepTaskComponentId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextStepTaskComponentId",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NextStepTaskComponentId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableDynamicStepTaskSelection",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "EnableDynamicStepTaskSelection",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "EnableDynamicStepTaskSelection",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableDynamicStepTaskSelection",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "NextStepTaskComponentId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "NextStepTaskComponentId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "NextStepTaskComponentId",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "NextStepTaskComponentId",
                schema: "public",
                table: "NtsService");
        }
    }
}
