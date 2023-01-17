using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221123_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantPhotoId",
                schema: "log",
                table: "BLSAppointmentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ApplicantPhotoId",
                schema: "public",
                table: "BLSAppointment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantPhotoId",
                schema: "log",
                table: "BLSAppointmentLog");

            migrationBuilder.DropColumn(
                name: "ApplicantPhotoId",
                schema: "public",
                table: "BLSAppointment");
        }
    }
}
