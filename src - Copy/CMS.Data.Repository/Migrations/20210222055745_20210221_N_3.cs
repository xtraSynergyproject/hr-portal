using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210221_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCopyofQID",
                schema: "rec",
                table: "CandidateProfile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QIDAttachmentId",
                schema: "rec",
                table: "CandidateProfile",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsCopyofQID",
                schema: "rec",
                table: "Application",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QIDAttachmentId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCopyofQID",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "QIDAttachmentId",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "IsCopyofQID",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "QIDAttachmentId",
                schema: "rec",
                table: "Application");
        }
    }
}
