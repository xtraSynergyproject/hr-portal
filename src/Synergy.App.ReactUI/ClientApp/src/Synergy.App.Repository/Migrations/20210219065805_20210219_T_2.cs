using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210219_T_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskStatusCode",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TaskStatusName",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskStatusCode",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TaskStatusName",
                schema: "public",
                table: "NtsTask");
        }
    }
}
