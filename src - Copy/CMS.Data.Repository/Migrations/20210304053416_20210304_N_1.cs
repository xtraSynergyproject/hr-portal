using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210304_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HRHeadApproval",
                schema: "rec",
                table: "Application",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HRHeadComment",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "HodApproval",
                schema: "rec",
                table: "Application",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HodComment",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "PlanningApproval",
                schema: "rec",
                table: "Application",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanningComment",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HRHeadApproval",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "HRHeadComment",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "HodApproval",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "HodComment",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "PlanningApproval",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "PlanningComment",
                schema: "rec",
                table: "Application");
        }
    }
}
