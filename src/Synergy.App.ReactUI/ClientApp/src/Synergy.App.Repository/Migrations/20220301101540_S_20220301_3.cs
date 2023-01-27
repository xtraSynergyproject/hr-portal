using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220301_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomRootId",
                schema: "log",
                table: "UserHierarchyPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CustomRootId",
                schema: "public",
                table: "UserHierarchyPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomRootId",
                schema: "log",
                table: "UserHierarchyPermissionLog");

            migrationBuilder.DropColumn(
                name: "CustomRootId",
                schema: "public",
                table: "UserHierarchyPermission");
        }
    }
}
