using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210216_N_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationExperienceByOther_CandidateProfile_CandidateProf~",
                schema: "rec",
                table: "ApplicationExperienceByOther");

            migrationBuilder.RenameColumn(
                name: "CandidateProfileId",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationExperienceByOther_CandidateProfileId",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                newName: "IX_ApplicationExperienceByOther_ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationExperienceByOther_Application_ApplicationId",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                column: "ApplicationId",
                principalSchema: "rec",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationExperienceByOther_Application_ApplicationId",
                schema: "rec",
                table: "ApplicationExperienceByOther");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                newName: "CandidateProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationExperienceByOther_ApplicationId",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                newName: "IX_ApplicationExperienceByOther_CandidateProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationExperienceByOther_CandidateProfile_CandidateProf~",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                column: "CandidateProfileId",
                principalSchema: "rec",
                principalTable: "CandidateProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
