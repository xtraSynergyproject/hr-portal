using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210317_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppointmentRemarks",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "FinalOfferReference",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "SalaryRevision",
                schema: "rec",
                table: "Application",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SalaryRevisionAmount",
                schema: "rec",
                table: "Application",
                type: "double precision",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentRemarks",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "FinalOfferReference",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "SalaryRevision",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "SalaryRevisionAmount",
                schema: "rec",
                table: "Application");
        }
    }
}
