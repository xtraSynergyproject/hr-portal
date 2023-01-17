using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210216_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndianSalary",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "OverseasSalary",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.RenameColumn(
                name: "OverseasSalary",
                schema: "rec",
                table: "Application",
                newName: "NetSalaryCurrency");

            migrationBuilder.RenameColumn(
                name: "IndianSalary",
                schema: "rec",
                table: "Application",
                newName: "ExpectedSalary");

            migrationBuilder.AddColumn<string>(
                name: "ExpectedCurrency",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedCurrency",
                schema: "rec",
                table: "Application");

            migrationBuilder.RenameColumn(
                name: "NetSalaryCurrency",
                schema: "rec",
                table: "Application",
                newName: "OverseasSalary");

            migrationBuilder.RenameColumn(
                name: "ExpectedSalary",
                schema: "rec",
                table: "Application",
                newName: "IndianSalary");

            migrationBuilder.AddColumn<string>(
                name: "IndianSalary",
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
        }
    }
}
