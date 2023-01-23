using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210216_15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CandidateEvaluation_ApplicationId",
                schema: "rec",
                table: "CandidateEvaluation",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateEvaluation_Application_ApplicationId",
                schema: "rec",
                table: "CandidateEvaluation",
                column: "ApplicationId",
                principalSchema: "rec",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateEvaluation_Application_ApplicationId",
                schema: "rec",
                table: "CandidateEvaluation");

            migrationBuilder.DropIndex(
                name: "IX_CandidateEvaluation_ApplicationId",
                schema: "rec",
                table: "CandidateEvaluation");
        }
    }
}
