using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class _20220505_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceId",
                schema: "log",
                table: "LOVLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceType",
                schema: "log",
                table: "LOVLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceId",
                schema: "log",
                table: "LOVLog");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                schema: "log",
                table: "LOVLog");
        }
    }
}
