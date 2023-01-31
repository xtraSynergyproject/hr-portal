using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210426_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TableSelectionType",
                schema: "public",
                table: "Template",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UdfUIType",
                schema: "public",
                table: "ColumnMetadata",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableSelectionType",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "UdfUIType",
                schema: "public",
                table: "ColumnMetadata");
        }
    }
}
