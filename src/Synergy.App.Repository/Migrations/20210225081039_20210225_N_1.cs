using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210225_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "Experience",
                schema: "rec",
                table: "JobAdvertisementTrack");

            migrationBuilder.DropColumn(
                name: "NoOfPosition",
                schema: "rec",
                table: "JobAdvertisementTrack");

            migrationBuilder.DropColumn(
                name: "Experience",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.DropColumn(
                name: "NoOfPosition",
                schema: "rec",
                table: "JobAdvertisement");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Experience",
                schema: "rec",
                table: "JobAdvertisementTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NoOfPosition",
                schema: "rec",
                table: "JobAdvertisementTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Experience",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NoOfPosition",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
