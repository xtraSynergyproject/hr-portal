using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210224_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Age",
                schema: "rec",
                table: "CandidateProfile",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Age",
                schema: "rec",
                table: "Application",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "Age",
                schema: "rec",
                table: "Application");
        }
    }
}
