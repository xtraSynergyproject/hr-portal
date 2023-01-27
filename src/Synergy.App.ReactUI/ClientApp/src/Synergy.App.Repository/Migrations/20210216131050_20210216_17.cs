using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210216_17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationId",
                schema: "rec",
                table: "AppointmentApprovalRequest",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentApprovalRequest_ApplicationId",
                schema: "rec",
                table: "AppointmentApprovalRequest",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentApprovalRequest_Application_ApplicationId",
                schema: "rec",
                table: "AppointmentApprovalRequest",
                column: "ApplicationId",
                principalSchema: "rec",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentApprovalRequest_Application_ApplicationId",
                schema: "rec",
                table: "AppointmentApprovalRequest");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentApprovalRequest_ApplicationId",
                schema: "rec",
                table: "AppointmentApprovalRequest");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                schema: "rec",
                table: "AppointmentApprovalRequest");
        }
    }
}
