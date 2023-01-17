using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class N_20220511_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "Street",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "Street",
                schema: "public",
                table: "LegalEntity");
        }
    }
}
