using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210219_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationStateTrack_Application_ApplicationId",
                schema: "rec",
                table: "ApplicationStateTrack");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationStateTrack_ApplicationId",
                schema: "rec",
                table: "ApplicationStateTrack");

            migrationBuilder.RenameColumn(
                name: "PassportStatus",
                schema: "rec",
                table: "Application",
                newName: "PassportStatusId");

            migrationBuilder.AddColumn<string>(
                name: "PassportStatusId",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassportStatusId",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.RenameColumn(
                name: "PassportStatusId",
                schema: "rec",
                table: "Application",
                newName: "PassportStatus");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStateTrack_ApplicationId",
                schema: "rec",
                table: "ApplicationStateTrack",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationStateTrack_Application_ApplicationId",
                schema: "rec",
                table: "ApplicationStateTrack",
                column: "ApplicationId",
                principalSchema: "rec",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
