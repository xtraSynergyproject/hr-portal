using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220713_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanceledWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DraftWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "InprogressWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OverdueWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RejectedWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CanceledWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DraftWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "InprogressWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OverdueWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RejectedWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CanceledWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DraftWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "InprogressWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OverdueWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RejectedWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnedWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CanceledWorkflowStatus",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DraftWorkflowStatus",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "InprogressWorkflowStatus",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OverdueWorkflowStatus",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RejectedWorkflowStatus",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnedWorkflowStatus",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanceledWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "DraftWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "InprogressWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "OverdueWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "RejectedWorkflowStatus",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "CanceledWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "DraftWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "InprogressWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "OverdueWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "RejectedWorkflowStatus",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "CanceledWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "DraftWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "InprogressWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "OverdueWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "RejectedWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "ReturnedWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "CanceledWorkflowStatus",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "DraftWorkflowStatus",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "InprogressWorkflowStatus",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "OverdueWorkflowStatus",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "RejectedWorkflowStatus",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ReturnedWorkflowStatus",
                schema: "public",
                table: "NtsService");
        }
    }
}
