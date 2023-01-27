using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210218_T_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_TaskTemplate_TemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceTypeCode",
                schema: "public",
                table: "NtsTaskVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTypeId",
                schema: "public",
                table: "NtsTaskVersion",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "public",
                table: "NtsTaskVersion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "public",
                table: "NtsTask",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceTypeCode",
                schema: "public",
                table: "NtsTaskVersion");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeId",
                schema: "public",
                table: "NtsTaskVersion");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "public",
                table: "NtsTaskVersion");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TemplateId",
                schema: "public",
                table: "NtsTask",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_TaskTemplate_TemplateId",
                schema: "public",
                table: "NtsTask",
                column: "TemplateId",
                principalSchema: "public",
                principalTable: "TaskTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
