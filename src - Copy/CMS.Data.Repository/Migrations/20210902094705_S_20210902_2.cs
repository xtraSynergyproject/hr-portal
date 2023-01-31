using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210902_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInheritedFromChild",
                schema: "log",
                table: "DocumentPermissionLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInheritedFromChild",
                schema: "public",
                table: "DocumentPermission",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInheritedFromChild",
                schema: "log",
                table: "DocumentPermissionLog");

            migrationBuilder.DropColumn(
                name: "IsInheritedFromChild",
                schema: "public",
                table: "DocumentPermission");
        }
    }
}
