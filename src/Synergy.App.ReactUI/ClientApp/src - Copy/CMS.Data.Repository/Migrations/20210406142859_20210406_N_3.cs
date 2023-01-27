using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210406_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentType",
                schema: "public",
                table: "NtsTask",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                schema: "public",
                table: "NtsTask",
                newName: "ParentTaskId");

            migrationBuilder.AddColumn<string>(
                name: "CloseComment",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentServiceId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CloseComment",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                schema: "public",
                table: "NtsService",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseComment",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ParentServiceId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "CloseComment",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "Rating",
                schema: "public",
                table: "NtsService");

            migrationBuilder.RenameColumn(
                name: "Rating",
                schema: "public",
                table: "NtsTask",
                newName: "ParentType");

            migrationBuilder.RenameColumn(
                name: "ParentTaskId",
                schema: "public",
                table: "NtsTask",
                newName: "ParentId");
        }
    }
}
