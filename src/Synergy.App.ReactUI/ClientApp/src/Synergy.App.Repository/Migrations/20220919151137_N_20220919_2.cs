using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class N_20220919_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TriggeredByReferenceId",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "TriggeredByReferenceType",
                schema: "log",
                table: "NtsServiceLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TriggeredByReferenceId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "TriggeredByReferenceType",
                schema: "public",
                table: "NtsService",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggeredByReferenceId",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "TriggeredByReferenceType",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "TriggeredByReferenceId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "TriggeredByReferenceType",
                schema: "public",
                table: "NtsService");
        }
    }
}
