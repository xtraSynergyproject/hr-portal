using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210303_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchType",
                schema: "rec",
                table: "Batch",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerBatchId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchType",
                schema: "rec",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "WorkerBatchId",
                schema: "rec",
                table: "Application");
        }
    }
}
