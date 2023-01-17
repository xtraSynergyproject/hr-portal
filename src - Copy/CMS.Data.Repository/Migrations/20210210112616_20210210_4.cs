using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210210_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PositionId",
                schema: "rec",
                table: "CandidateExperience",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                schema: "rec",
                table: "CandidateExperience",
                newName: "JobTitle");

            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                schema: "rec",
                table: "CandidateEducational",
                newName: "Specialization");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "rec",
                table: "CandidateDrivingLicense",
                newName: "LicenseType");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                schema: "rec",
                table: "ApplicationExperience",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                schema: "rec",
                table: "ApplicationExperience",
                newName: "JobTitle");

            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                schema: "rec",
                table: "ApplicationEducational",
                newName: "Specialization");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "rec",
                table: "ApplicationDrivingLicense",
                newName: "LicenseType");

            migrationBuilder.AddColumn<string>(
                name: "CurrentAddress",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "HeardAboutUsFrom",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IndianSalary",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NetSalary",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NoticePeriod",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherAllowances",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherCountryVisa",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "OtherCountryVisaExpiry",
                schema: "rec",
                table: "CandidateProfile",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherCountryVisaType",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherVisaType",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OverseasSalary",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "QatarNocAvailable",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "QatarNocNotAvailableReason",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

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
                name: "TotalWorkExperience",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "VisaCountry",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "VisaExpiry",
                schema: "rec",
                table: "CandidateProfile",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisaType",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<string>(
                name: "PassingYear",
                schema: "rec",
                table: "CandidateEducational",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Marks",
                schema: "rec",
                table: "CandidateEducational",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<string>(
                name: "PassingYear",
                schema: "rec",
                table: "ApplicationEducational",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Marks",
                schema: "rec",
                table: "ApplicationEducational",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CurrentAddress",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "HeardAboutUsFrom",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IndianSalary",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NetSalary",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NoticePeriod",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherAllowances",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherCountryVisa",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "OtherCountryVisaExpiry",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherCountryVisaType",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherVisaType",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OverseasSalary",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "QatarNocAvailable",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "QatarNocNotAvailableReason",
                schema: "rec",
                table: "Application",
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

            migrationBuilder.AddColumn<string>(
                name: "TotalWorkExperience",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "VisaCountry",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "VisaExpiry",
                schema: "rec",
                table: "Application",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisaType",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAddress",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "Designation",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "HeardAboutUsFrom",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "IndianSalary",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "NetSalary",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "NoticePeriod",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "OtherAllowances",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "OtherCountryVisa",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "OtherCountryVisaExpiry",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "OtherCountryVisaType",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "OtherVisaType",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "OverseasSalary",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "QatarNocAvailable",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "QatarNocNotAvailableReason",
                schema: "rec",
                table: "CandidateProfile");

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
                name: "TotalWorkExperience",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "VisaCountry",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "VisaExpiry",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "VisaType",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "CurrentAddress",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "Designation",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "HeardAboutUsFrom",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "IndianSalary",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "NetSalary",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "NoticePeriod",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherAllowances",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherCountryVisa",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherCountryVisaExpiry",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherCountryVisaType",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherVisaType",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OverseasSalary",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "QatarNocAvailable",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "QatarNocNotAvailableReason",
                schema: "rec",
                table: "Application");

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

            migrationBuilder.DropColumn(
                name: "TotalWorkExperience",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "VisaCountry",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "VisaExpiry",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "VisaType",
                schema: "rec",
                table: "Application");

            migrationBuilder.RenameColumn(
                name: "Location",
                schema: "rec",
                table: "CandidateExperience",
                newName: "PositionId");

            migrationBuilder.RenameColumn(
                name: "JobTitle",
                schema: "rec",
                table: "CandidateExperience",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "Specialization",
                schema: "rec",
                table: "CandidateEducational",
                newName: "SpecializationId");

            migrationBuilder.RenameColumn(
                name: "LicenseType",
                schema: "rec",
                table: "CandidateDrivingLicense",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Location",
                schema: "rec",
                table: "ApplicationExperience",
                newName: "PositionId");

            migrationBuilder.RenameColumn(
                name: "JobTitle",
                schema: "rec",
                table: "ApplicationExperience",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "Specialization",
                schema: "rec",
                table: "ApplicationEducational",
                newName: "SpecializationId");

            migrationBuilder.RenameColumn(
                name: "LicenseType",
                schema: "rec",
                table: "ApplicationDrivingLicense",
                newName: "Type");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassingYear",
                schema: "rec",
                table: "CandidateEducational",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<double>(
                name: "Marks",
                schema: "rec",
                table: "CandidateEducational",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassingYear",
                schema: "rec",
                table: "ApplicationEducational",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<double>(
                name: "Marks",
                schema: "rec",
                table: "ApplicationEducational",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
