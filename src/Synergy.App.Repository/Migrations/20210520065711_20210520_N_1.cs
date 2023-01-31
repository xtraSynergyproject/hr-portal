using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210520_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pop3Host",
                schema: "public",
                table: "ProjectEmailSetup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "Pop3Port",
                schema: "public",
                table: "ProjectEmailSetup",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pop3Host",
                schema: "public",
                table: "ProjectEmailSetup");

            migrationBuilder.DropColumn(
                name: "Pop3Port",
                schema: "public",
                table: "ProjectEmailSetup");
        }
    }
}
