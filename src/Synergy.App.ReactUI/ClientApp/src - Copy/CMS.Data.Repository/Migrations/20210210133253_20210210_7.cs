using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210210_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Transfer",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Requirement",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Planning",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Available",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion");

            migrationBuilder.DropColumn(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.DropColumn(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummary");

            migrationBuilder.AlterColumn<int>(
                name: "Transfer",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Requirement",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Planning",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Available",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
