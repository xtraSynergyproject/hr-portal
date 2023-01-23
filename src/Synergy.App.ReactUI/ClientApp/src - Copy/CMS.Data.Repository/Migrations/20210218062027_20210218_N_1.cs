using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210218_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecruitmentCandidateElementInfo_Application_ApplicationId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.DropIndex(
                name: "IX_RecruitmentCandidateElementInfo_ApplicationId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.AddColumn<string>(
                name: "InterviewByUserId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "InterviewDate",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterviewByUserId",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "InterviewDate",
                schema: "rec",
                table: "Application");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentCandidateElementInfo_ApplicationId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecruitmentCandidateElementInfo_Application_ApplicationId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                column: "ApplicationId",
                principalSchema: "rec",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
