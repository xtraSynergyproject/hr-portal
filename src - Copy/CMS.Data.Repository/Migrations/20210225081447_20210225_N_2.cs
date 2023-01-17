using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210225_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Experience",
                schema: "rec",
                table: "JobAdvertisementTrack",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NoOfPosition",
                schema: "rec",
                table: "JobAdvertisementTrack",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Experience",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NoOfPosition",
                schema: "rec",
                table: "JobAdvertisement",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
