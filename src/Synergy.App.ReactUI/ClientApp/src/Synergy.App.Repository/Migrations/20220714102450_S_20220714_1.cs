using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220714_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanceledWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "DraftWorkflowStatus",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "EnableFifo",
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
                name: "EnableFifo",
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

            migrationBuilder.AddColumn<string>(
                name: "CanceledWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DraftWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InprogressWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OverdueWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RejectedWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnedWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CanceledWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DraftWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InprogressWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OverdueWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RejectedWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ReturnedWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanceledWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DraftWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableFifo",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "InprogressWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "OverdueWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "RejectedWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "ReturnedWorkflowStatus",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "CanceledWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DraftWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EnableFifo",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "InprogressWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "OverdueWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "RejectedWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "ReturnedWorkflowStatus",
                schema: "public",
                table: "ServiceTemplate");

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

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "log",
                table: "NtsServiceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "public",
                table: "NtsService",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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
    }
}
