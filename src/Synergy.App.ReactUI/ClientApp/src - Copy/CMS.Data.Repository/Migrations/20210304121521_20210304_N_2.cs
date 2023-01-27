using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210304_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EDApproval",
                schema: "rec",
                table: "Application",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EDComment",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EDApproval",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "EDComment",
                schema: "rec",
                table: "Application");
        }
    }
}
