using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20220301_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HierarchyPath",
                schema: "log",
                table: "HybridHierarchyLog");

            migrationBuilder.DropColumn(
                name: "HierarchyPath",
                schema: "public",
                table: "HybridHierarchy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HierarchyPath",
                schema: "log",
                table: "HybridHierarchyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "HierarchyPath",
                schema: "public",
                table: "HybridHierarchy",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
