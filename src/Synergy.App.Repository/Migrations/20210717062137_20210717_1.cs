using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210717_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceType",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceType",
                schema: "public",
                table: "NtsService",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceType",
                schema: "public",
                table: "NtsNote",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                schema: "public",
                table: "NtsNote");
        }
    }
}
