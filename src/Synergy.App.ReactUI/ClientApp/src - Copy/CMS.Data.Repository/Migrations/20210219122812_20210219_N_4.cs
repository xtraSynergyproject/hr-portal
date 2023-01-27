using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210219_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationBeneficiary_Application_ApplicationId",
                schema: "rec",
                table: "ApplicationBeneficiary");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationBeneficiary_ApplicationId",
                schema: "rec",
                table: "ApplicationBeneficiary");

            migrationBuilder.DropColumn(
                name: "TotalGCCExperience",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "TotalOtherExperience",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "TotalQatarExperience",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "TotalGCCExperience",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "TotalOtherExperience",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "TotalQatarExperience",
                schema: "rec",
                table: "Application");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TotalGCCExperience",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TotalOtherExperience",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TotalQatarExperience",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TotalGCCExperience",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TotalOtherExperience",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TotalQatarExperience",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationBeneficiary_ApplicationId",
                schema: "rec",
                table: "ApplicationBeneficiary",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationBeneficiary_Application_ApplicationId",
                schema: "rec",
                table: "ApplicationBeneficiary",
                column: "ApplicationId",
                principalSchema: "rec",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
