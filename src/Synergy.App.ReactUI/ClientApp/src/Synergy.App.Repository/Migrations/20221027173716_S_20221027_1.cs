using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221027_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentComponentResultId",
                schema: "log",
                table: "ComponentResultLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentComponentResultId",
                schema: "public",
                table: "ComponentResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResultLog_ParentComponentResultId",
                schema: "log",
                table: "ComponentResultLog",
                column: "ParentComponentResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResult_ParentComponentResultId",
                schema: "public",
                table: "ComponentResult",
                column: "ParentComponentResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentResult_ComponentResult_ParentComponentResultId",
                schema: "public",
                table: "ComponentResult",
                column: "ParentComponentResultId",
                principalSchema: "public",
                principalTable: "ComponentResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentResultLog_ComponentResult_ParentComponentResultId",
                schema: "log",
                table: "ComponentResultLog",
                column: "ParentComponentResultId",
                principalSchema: "public",
                principalTable: "ComponentResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentResult_ComponentResult_ParentComponentResultId",
                schema: "public",
                table: "ComponentResult");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentResultLog_ComponentResult_ParentComponentResultId",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropIndex(
                name: "IX_ComponentResultLog_ParentComponentResultId",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropIndex(
                name: "IX_ComponentResult_ParentComponentResultId",
                schema: "public",
                table: "ComponentResult");

            migrationBuilder.DropColumn(
                name: "ParentComponentResultId",
                schema: "log",
                table: "ComponentResultLog");

            migrationBuilder.DropColumn(
                name: "ParentComponentResultId",
                schema: "public",
                table: "ComponentResult");
        }
    }
}
