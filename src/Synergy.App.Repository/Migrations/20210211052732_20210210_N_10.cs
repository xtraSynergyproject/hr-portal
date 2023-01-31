using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210210_N_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Transfer",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Requirement",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Planning",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Balance",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Available",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Transfer",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Requirement",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Planning",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Balance",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Available",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Transfer",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Requirement",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Planning",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Available",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Transfer",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Requirement",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Planning",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Available",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
