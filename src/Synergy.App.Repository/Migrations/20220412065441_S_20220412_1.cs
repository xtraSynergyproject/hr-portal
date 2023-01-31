using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220412_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkflowVisibility",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowVisibility",
                schema: "public",
                table: "StepTaskComponent",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowVisibility",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowVisibility",
                schema: "public",
                table: "ServiceTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HideMainMenu",
                schema: "log",
                table: "PortalLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideMainMenu",
                schema: "public",
                table: "Portal",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkflowVisibility",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "WorkflowVisibility",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "WorkflowVisibility",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "WorkflowVisibility",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideMainMenu",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "HideMainMenu",
                schema: "public",
                table: "Portal");
        }
    }
}
