using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221026_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RuntimeWorkflowDataId",
                schema: "log",
                table: "ComponentResultLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RuntimeWorkflowDataId",
                schema: "public",
                table: "ComponentResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResultLog_RuntimeWorkflowDataId",
                schema: "log",
                table: "ComponentResultLog",
                column: "RuntimeWorkflowDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResult_RuntimeWorkflowDataId",
                schema: "public",
                table: "ComponentResult",
                column: "RuntimeWorkflowDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentResult_RuntimeWorkflowData_RuntimeWorkflowDataId",
                schema: "public",
                table: "ComponentResult",
                column: "RuntimeWorkflowDataId",
                principalSchema: "public",
                principalTable: "RuntimeWorkflowData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentResultLog_RuntimeWorkflowData_RuntimeWorkflowDataId",
                schema: "log",
                table: "ComponentResultLog",
                column: "RuntimeWorkflowDataId",
                principalSchema: "public",
                principalTable: "RuntimeWorkflowData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentResult_RuntimeWorkflowData_RuntimeWorkflowDataId",
                schema: "public",
                table: "ComponentResult");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentResultLog_RuntimeWorkflowData_RuntimeWorkflowDataId",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropIndex(
                name: "IX_ComponentResultLog_RuntimeWorkflowDataId",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropIndex(
                name: "IX_ComponentResult_RuntimeWorkflowDataId",
                schema: "public",
                table: "ComponentResult");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowDataId",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropColumn(
                name: "RuntimeWorkflowDataId",
                schema: "public",
                table: "ComponentResult");
        }
    }
}
