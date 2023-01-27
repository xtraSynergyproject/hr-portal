using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210224_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialization",
                schema: "rec",
                table: "CandidateEducational",
                newName: "SpecializationId");

            migrationBuilder.RenameColumn(
                name: "Specialization",
                schema: "rec",
                table: "ApplicationEducational",
                newName: "SpecializationId");

            migrationBuilder.AddColumn<bool>(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceDetailsHeight",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherEducationType",
                schema: "rec",
                table: "CandidateEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherSpecialization",
                schema: "rec",
                table: "CandidateEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherEducationType",
                schema: "rec",
                table: "ApplicationEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherQualification",
                schema: "rec",
                table: "ApplicationEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherSpecialization",
                schema: "rec",
                table: "ApplicationEducational",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ServiceDetailsHeight",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "OtherEducationType",
                schema: "rec",
                table: "CandidateEducational");

            migrationBuilder.DropColumn(
                name: "OtherSpecialization",
                schema: "rec",
                table: "CandidateEducational");

            migrationBuilder.DropColumn(
                name: "OtherEducationType",
                schema: "rec",
                table: "ApplicationEducational");

            migrationBuilder.DropColumn(
                name: "OtherQualification",
                schema: "rec",
                table: "ApplicationEducational");

            migrationBuilder.DropColumn(
                name: "OtherSpecialization",
                schema: "rec",
                table: "ApplicationEducational");

            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                schema: "rec",
                table: "CandidateEducational",
                newName: "Specialization");

            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                schema: "rec",
                table: "ApplicationEducational",
                newName: "Specialization");
        }
    }
}
