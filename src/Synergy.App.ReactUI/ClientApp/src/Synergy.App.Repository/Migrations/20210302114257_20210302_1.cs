using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210302_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescribeHowHeSuits",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "InterviewVenue",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LeaveCycle",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NewPostJustification",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherBenefits",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SelectedThroughId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescribeHowHeSuits",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "InterviewVenue",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "LeaveCycle",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "NewPostJustification",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherBenefits",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "SelectedThroughId",
                schema: "rec",
                table: "Application");
        }
    }
}
