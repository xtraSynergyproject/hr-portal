using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210619_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RootNodeId",
                schema: "log",
                table: "HierarchyMasterLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RootNodeId",
                schema: "public",
                table: "HierarchyMaster",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootNodeId",
                schema: "log",
                table: "HierarchyMasterLog");

            migrationBuilder.DropColumn(
                name: "RootNodeId",
                schema: "public",
                table: "HierarchyMaster");
        }
    }
}
