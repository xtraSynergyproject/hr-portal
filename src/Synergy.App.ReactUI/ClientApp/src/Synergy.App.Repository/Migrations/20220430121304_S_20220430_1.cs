using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220430_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CriteriaType",
                schema: "rec",
                table: "JobDescriptionCriteria",
                newName: "CriteriaTypeId");

            migrationBuilder.RenameColumn(
                name: "Qualification",
                schema: "rec",
                table: "JobDescription",
                newName: "QualificationId");

            migrationBuilder.RenameColumn(
                name: "CriteriaType",
                schema: "rec",
                table: "JobCriteria",
                newName: "CriteriaTypeId");

            migrationBuilder.RenameColumn(
                name: "Qualification",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "QualificationId");

            migrationBuilder.RenameColumn(
                name: "NeededDate",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "RequiredDate");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "JobLocationId");

            migrationBuilder.AddColumn<string>(
                name: "CandidateId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidateId",
                schema: "rec",
                table: "Application");

            migrationBuilder.RenameColumn(
                name: "CriteriaTypeId",
                schema: "rec",
                table: "JobDescriptionCriteria",
                newName: "CriteriaType");

            migrationBuilder.RenameColumn(
                name: "QualificationId",
                schema: "rec",
                table: "JobDescription",
                newName: "Qualification");

            migrationBuilder.RenameColumn(
                name: "CriteriaTypeId",
                schema: "rec",
                table: "JobCriteria",
                newName: "CriteriaType");

            migrationBuilder.RenameColumn(
                name: "RequiredDate",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "NeededDate");

            migrationBuilder.RenameColumn(
                name: "QualificationId",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "Qualification");

            migrationBuilder.RenameColumn(
                name: "JobLocationId",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "LocationId");
        }
    }
}
