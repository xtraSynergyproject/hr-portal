using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210228_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiometricCompleted",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "CandidateArrived",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "CandidateJoined",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "FinalOfferSent",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "InterviewCompleted",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "MedicalCompleted",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "NoOfApplication",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "VisaApproved",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "VisaTransfer",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "PositionId",
                schema: "rec",
                table: "Application");

            migrationBuilder.AddColumn<int>(
                name: "CreateReturnType",
                schema: "public",
                table: "NoteTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DisableVersioning",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EditReturnType",
                schema: "public",
                table: "NoteTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SaveNewVersionButtonCss",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SaveNewVersionButtonText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<long>(
                name: "Seperation",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Seperation",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Agency",
                schema: "rec",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CountryId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AgencyName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ContactPersonName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ContactEmail = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ContactNumber = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agency_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate",
                column: "OrderByColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Agency_UserId",
                schema: "rec",
                table: "Agency",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate",
                column: "OrderByColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropTable(
                name: "Agency",
                schema: "rec");

            migrationBuilder.DropIndex(
                name: "IX_NoteIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "CreateReturnType",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "DisableVersioning",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "EditReturnType",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "SaveNewVersionButtonCss",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "SaveNewVersionButtonText",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "Seperation",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "Seperation",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.AddColumn<long>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CandidateArrived",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CandidateJoined",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinalOfferSent",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NoOfApplication",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaApproved",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaTransfer",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositionId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
