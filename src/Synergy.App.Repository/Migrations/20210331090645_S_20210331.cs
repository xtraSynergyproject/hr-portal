using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210331 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowSLAChange",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowSLAChange",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PriorityId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SLA",
                schema: "public",
                table: "StepTaskComponent",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowSLAChange",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PriorityId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SLA",
                schema: "public",
                table: "ServiceTemplate",
                type: "interval",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_PriorityId",
                schema: "public",
                table: "StepTaskComponent",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_PriorityId",
                schema: "public",
                table: "ServiceTemplate",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_LOV_PriorityId",
                schema: "public",
                table: "ServiceTemplate",
                column: "PriorityId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_LOV_PriorityId",
                schema: "public",
                table: "StepTaskComponent",
                column: "PriorityId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_LOV_PriorityId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_LOV_PriorityId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_PriorityId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_PriorityId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "AllowSLAChange",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AllowSLAChange",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "SLA",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "AllowSLAChange",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "SLA",
                schema: "public",
                table: "ServiceTemplate");
        }
    }
}
