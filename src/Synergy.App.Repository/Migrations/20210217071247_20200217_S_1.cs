using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20200217_S_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CandidateProfile_UserId",
                schema: "rec",
                table: "CandidateProfile",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateProfile_User_UserId",
                schema: "rec",
                table: "CandidateProfile",
                column: "UserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateProfile_User_UserId",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropIndex(
                name: "IX_CandidateProfile_UserId",
                schema: "rec",
                table: "CandidateProfile");
        }
    }
}
