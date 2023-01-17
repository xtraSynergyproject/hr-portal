using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210711_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableReOpenButton",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TaskAssignedToTypeId",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableReOpenButton",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TaskAssignedToTypeId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplateLog_TaskAssignedToTypeId",
                schema: "log",
                table: "TaskTemplateLog",
                column: "TaskAssignedToTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplate_TaskAssignedToTypeId",
                schema: "public",
                table: "TaskTemplate",
                column: "TaskAssignedToTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplate_LOV_TaskAssignedToTypeId",
                schema: "public",
                table: "TaskTemplate",
                column: "TaskAssignedToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplateLog_LOV_TaskAssignedToTypeId",
                schema: "log",
                table: "TaskTemplateLog",
                column: "TaskAssignedToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplate_LOV_TaskAssignedToTypeId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplateLog_LOV_TaskAssignedToTypeId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplateLog_TaskAssignedToTypeId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplate_TaskAssignedToTypeId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableReOpenButton",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "TaskAssignedToTypeId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableReOpenButton",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TaskAssignedToTypeId",
                schema: "public",
                table: "TaskTemplate");
        }
    }
}
