using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210408_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedToHierarchyMasterId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToHierarchyMasterLevelId",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToHierarchyMasterId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "AssignedToHierarchyMasterLevelId",
                schema: "public",
                table: "NtsTask");
        }
    }
}
