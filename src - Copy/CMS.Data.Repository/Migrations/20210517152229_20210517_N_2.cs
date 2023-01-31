using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210517_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmtpPort",
                schema: "public",
                table: "ProjectEmailSetup");

            migrationBuilder.AddColumn<string>(
                name: "FavIconId",
                schema: "log",
                table: "PortalLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "FavIconId",
                schema: "public",
                table: "Portal",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavIconId",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "FavIconId",
                schema: "public",
                table: "Portal");

            migrationBuilder.AddColumn<string>(
                name: "SmtpPort",
                schema: "public",
                table: "ProjectEmailSetup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
