using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20220207_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BulkRequestId",
                schema: "log",
                table: "HybridHierarchyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "BulkRequestId",
                schema: "public",
                table: "HybridHierarchy",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BulkRequestId",
                schema: "log",
                table: "HybridHierarchyLog");

            migrationBuilder.DropColumn(
                name: "BulkRequestId",
                schema: "public",
                table: "HybridHierarchy");
        }
    }
}
