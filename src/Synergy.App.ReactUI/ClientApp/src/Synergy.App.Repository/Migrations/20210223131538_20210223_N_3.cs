using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210223_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisement_ManpowerRecruitmentSummary_ManpowerRecruit~",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropIndex(
                name: "IX_JobAdvertisement_ManpowerRecruitmentSummaryId",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "ManpowerRecruitmentSummaryId",
                schema: "rec",
                table: "JobAdvertisement");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManpowerRecruitmentSummaryId",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisement_ManpowerRecruitmentSummaryId",
                schema: "rec",
                table: "JobAdvertisement",
                column: "ManpowerRecruitmentSummaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisement_ManpowerRecruitmentSummary_ManpowerRecruit~",
                schema: "rec",
                table: "JobAdvertisement",
                column: "ManpowerRecruitmentSummaryId",
                principalSchema: "rec",
                principalTable: "ManpowerRecruitmentSummary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
