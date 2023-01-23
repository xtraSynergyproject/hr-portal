using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210218_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedToTeamId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToUserId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AccommodationId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToTeamId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AccommodationId",
                schema: "rec",
                table: "Application");
        }
    }
}
