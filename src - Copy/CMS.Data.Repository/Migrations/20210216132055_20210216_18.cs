using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210216_18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEvaluationScale4",
                schema: "rec",
                table: "CandidateEvaluation",
                newName: "IsEvaluationScale3");

            migrationBuilder.AddColumn<string>(
                name: "InterviewVenue",
                schema: "rec",
                table: "AppointmentApprovalRequest",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterviewVenue",
                schema: "rec",
                table: "AppointmentApprovalRequest");

            migrationBuilder.RenameColumn(
                name: "IsEvaluationScale3",
                schema: "rec",
                table: "CandidateEvaluation",
                newName: "IsEvaluationScale4");
        }
    }
}
