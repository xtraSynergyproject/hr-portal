using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210220_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationExperienceByOther_Application_ApplicationId",
                schema: "rec",
                table: "ApplicationExperienceByOther");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationExperienceByOther_ApplicationId",
                schema: "rec",
                table: "ApplicationExperienceByOther");

            migrationBuilder.DropColumn(
                name: "ConstructionPeriodFrom",
                schema: "rec",
                table: "CandidateProject");

            migrationBuilder.DropColumn(
                name: "ConstructionPeriodTo",
                schema: "rec",
                table: "CandidateProject");

            migrationBuilder.DropColumn(
                name: "ConstructionPeriodFrom",
                schema: "rec",
                table: "ApplicationProject");

            migrationBuilder.DropColumn(
                name: "ConstructionPeriodTo",
                schema: "rec",
                table: "ApplicationProject");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConstructionPeriodFrom",
                schema: "rec",
                table: "CandidateProject",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ConstructionPeriodTo",
                schema: "rec",
                table: "CandidateProject",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ConstructionPeriodFrom",
                schema: "rec",
                table: "ApplicationProject",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ConstructionPeriodTo",
                schema: "rec",
                table: "ApplicationProject",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationExperienceByOther_ApplicationId",
                schema: "rec",
                table: "ApplicationExperienceByOther",
                column: "ApplicationId");

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
    }
}
