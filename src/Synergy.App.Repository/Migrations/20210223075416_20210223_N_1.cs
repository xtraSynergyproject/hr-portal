using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210223_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualExperience",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ActualITSkills",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ActualQualification",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ActualSpecialization",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ActualTechnical",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AnyOtherLanguage",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CountriesWorked",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicense",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ExtraCurricular",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "FieldOfExposure",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NatureOfWork",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationWorked",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PositionsWorked",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequirementExperience",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequirementITSkills",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequirementQualification",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequirementSpecialization",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequirementTechnical",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TrainingsUndergone",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualExperience",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ActualITSkills",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ActualQualification",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ActualSpecialization",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ActualTechnical",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "AnyOtherLanguage",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "CountriesWorked",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "DrivingLicense",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ExtraCurricular",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "FieldOfExposure",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "NatureOfWork",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OrganizationWorked",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "PositionsWorked",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "RequirementExperience",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "RequirementITSkills",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "RequirementQualification",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "RequirementSpecialization",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "RequirementTechnical",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "TrainingsUndergone",
                schema: "rec",
                table: "Application");
        }
    }
}
