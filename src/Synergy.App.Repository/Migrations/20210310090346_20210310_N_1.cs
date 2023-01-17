using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210310_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalWorkExperience",
                schema: "rec",
                table: "Application");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTableSchemaName",
                schema: "public",
                table: "ColumnMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<double>(
                name: "Duration",
                schema: "rec",
                table: "CandidateExperience",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Duration",
                schema: "rec",
                table: "ApplicationExperience",
                type: "double precision",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceTableSchemaName",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "rec",
                table: "CandidateExperience");

            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "rec",
                table: "ApplicationExperience");

            migrationBuilder.AddColumn<string>(
                name: "TotalWorkExperience",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
