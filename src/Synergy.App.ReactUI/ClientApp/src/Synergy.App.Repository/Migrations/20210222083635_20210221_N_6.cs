using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210221_N_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherNextOfApplicantRelationship",
                schema: "rec",
                table: "Application",
                newName: "OtherNextOfKinRelationship");

            migrationBuilder.RenameColumn(
                name: "OtherNextOfApplicantPhoneNo",
                schema: "rec",
                table: "Application",
                newName: "OtherNextOfKinPhoneNo");

            migrationBuilder.RenameColumn(
                name: "OtherNextOfApplicantEmail",
                schema: "rec",
                table: "Application",
                newName: "OtherNextOfKinEmail");

            migrationBuilder.RenameColumn(
                name: "OtherNextOfApplicant",
                schema: "rec",
                table: "Application",
                newName: "OtherNextOfKin");

            migrationBuilder.RenameColumn(
                name: "NextOfApplicantRelationship",
                schema: "rec",
                table: "Application",
                newName: "NextOfKinRelationship");

            migrationBuilder.RenameColumn(
                name: "NextOfApplicantPhoneNo",
                schema: "rec",
                table: "Application",
                newName: "NextOfKinPhoneNo");

            migrationBuilder.RenameColumn(
                name: "NextOfApplicantEmail",
                schema: "rec",
                table: "Application",
                newName: "NextOfKinEmail");

            migrationBuilder.RenameColumn(
                name: "NextOfApplicant",
                schema: "rec",
                table: "Application",
                newName: "NextOfKin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherNextOfKinRelationship",
                schema: "rec",
                table: "Application",
                newName: "OtherNextOfApplicantRelationship");

            migrationBuilder.RenameColumn(
                name: "OtherNextOfKinPhoneNo",
                schema: "rec",
                table: "Application",
                newName: "OtherNextOfApplicantPhoneNo");

            migrationBuilder.RenameColumn(
                name: "OtherNextOfKinEmail",
                schema: "rec",
                table: "Application",
                newName: "OtherNextOfApplicantEmail");

            migrationBuilder.RenameColumn(
                name: "OtherNextOfKin",
                schema: "rec",
                table: "Application",
                newName: "OtherNextOfApplicant");

            migrationBuilder.RenameColumn(
                name: "NextOfKinRelationship",
                schema: "rec",
                table: "Application",
                newName: "NextOfApplicantRelationship");

            migrationBuilder.RenameColumn(
                name: "NextOfKinPhoneNo",
                schema: "rec",
                table: "Application",
                newName: "NextOfApplicantPhoneNo");

            migrationBuilder.RenameColumn(
                name: "NextOfKinEmail",
                schema: "rec",
                table: "Application",
                newName: "NextOfApplicantEmail");

            migrationBuilder.RenameColumn(
                name: "NextOfKin",
                schema: "rec",
                table: "Application",
                newName: "NextOfApplicant");
        }
    }
}
