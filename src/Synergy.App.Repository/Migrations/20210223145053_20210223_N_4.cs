using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210223_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<long>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummaryVersion",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BiometricCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CandidateArrived",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CandidateJoined",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FightTicketsBooked",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinalOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IntentToOfferAccepted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IntentToOfferSent",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InterviewCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MedicalCompleted",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NoOfApplication",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShortlistedByHr",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShortlistedForInterview",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaAppointmentTaken",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaApproved",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisaSentToCandidates",
                schema: "rec",
                table: "ManpowerRecruitmentSummary",
                type: "bigint",
                nullable: true);
        }
    }
}
