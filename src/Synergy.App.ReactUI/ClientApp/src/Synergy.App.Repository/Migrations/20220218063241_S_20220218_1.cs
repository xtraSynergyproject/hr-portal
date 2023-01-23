using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220218_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContributionType",
                schema: "log",
                table: "NtsTaskSharedLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContributionType",
                schema: "public",
                table: "NtsTaskShared",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContributionType",
                schema: "log",
                table: "NtsServiceSharedLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContributionType",
                schema: "public",
                table: "NtsServiceShared",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContributionType",
                schema: "log",
                table: "NtsNoteSharedLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContributionType",
                schema: "public",
                table: "NtsNoteShared",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContributionType",
                schema: "log",
                table: "NtsTaskSharedLog");

            migrationBuilder.DropColumn(
                name: "ContributionType",
                schema: "public",
                table: "NtsTaskShared");

            migrationBuilder.DropColumn(
                name: "ContributionType",
                schema: "log",
                table: "NtsServiceSharedLog");

            migrationBuilder.DropColumn(
                name: "ContributionType",
                schema: "public",
                table: "NtsServiceShared");

            migrationBuilder.DropColumn(
                name: "ContributionType",
                schema: "log",
                table: "NtsNoteSharedLog");

            migrationBuilder.DropColumn(
                name: "ContributionType",
                schema: "public",
                table: "NtsNoteShared");
        }
    }
}
