using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class N_20220914_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StepTaskEscalationId",
                schema: "log",
                table: "StepTaskEscalationDataLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StepTaskEscalationId",
                schema: "public",
                table: "StepTaskEscalationData",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationDataLog_StepTaskEscalationId",
                schema: "log",
                table: "StepTaskEscalationDataLog",
                column: "StepTaskEscalationId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationData_StepTaskEscalationId",
                schema: "public",
                table: "StepTaskEscalationData",
                column: "StepTaskEscalationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskEscalationData_StepTaskEscalation_StepTaskEscalatio~",
                schema: "public",
                table: "StepTaskEscalationData",
                column: "StepTaskEscalationId",
                principalSchema: "public",
                principalTable: "StepTaskEscalation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskEscalationDataLog_StepTaskEscalation_StepTaskEscala~",
                schema: "log",
                table: "StepTaskEscalationDataLog",
                column: "StepTaskEscalationId",
                principalSchema: "public",
                principalTable: "StepTaskEscalation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskEscalationData_StepTaskEscalation_StepTaskEscalatio~",
                schema: "public",
                table: "StepTaskEscalationData");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskEscalationDataLog_StepTaskEscalation_StepTaskEscala~",
                schema: "log",
                table: "StepTaskEscalationDataLog");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskEscalationDataLog_StepTaskEscalationId",
                schema: "log",
                table: "StepTaskEscalationDataLog");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskEscalationData_StepTaskEscalationId",
                schema: "public",
                table: "StepTaskEscalationData");

            migrationBuilder.DropColumn(
                name: "StepTaskEscalationId",
                schema: "log",
                table: "StepTaskEscalationDataLog");

            migrationBuilder.DropColumn(
                name: "StepTaskEscalationId",
                schema: "public",
                table: "StepTaskEscalationData");
        }
    }
}
