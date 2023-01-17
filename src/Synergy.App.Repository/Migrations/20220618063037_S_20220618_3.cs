using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220618_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StepTaskComponentId",
                schema: "log",
                table: "StepTaskSkipLogicLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepTaskComponentId",
                schema: "public",
                table: "StepTaskSkipLogic",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepTaskComponentId",
                schema: "log",
                table: "StepTaskAssigneeLogicLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepTaskComponentId",
                schema: "public",
                table: "StepTaskAssigneeLogic",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StepTaskComponentId",
                schema: "log",
                table: "StepTaskSkipLogicLog");

            migrationBuilder.DropColumn(
                name: "StepTaskComponentId",
                schema: "public",
                table: "StepTaskSkipLogic");

            migrationBuilder.DropColumn(
                name: "StepTaskComponentId",
                schema: "log",
                table: "StepTaskAssigneeLogicLog");

            migrationBuilder.DropColumn(
                name: "StepTaskComponentId",
                schema: "public",
                table: "StepTaskAssigneeLogic");
        }
    }
}
