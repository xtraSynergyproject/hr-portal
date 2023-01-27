using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210728_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "log",
                table: "TeamLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "TeamType",
                schema: "log",
                table: "TeamLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamWorkAssignmentType",
                schema: "log",
                table: "TeamLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "public",
                table: "Team",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "TeamType",
                schema: "public",
                table: "Team",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamWorkAssignmentType",
                schema: "public",
                table: "Team",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "log",
                table: "TeamLog");

            migrationBuilder.DropColumn(
                name: "TeamType",
                schema: "log",
                table: "TeamLog");

            migrationBuilder.DropColumn(
                name: "TeamWorkAssignmentType",
                schema: "log",
                table: "TeamLog");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "public",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "TeamType",
                schema: "public",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "TeamWorkAssignmentType",
                schema: "public",
                table: "Team");
        }
    }
}
