using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210311_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyDisplayColumnReferenceId",
                schema: "public",
                table: "ColumnMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKeyDisplayColumn",
                schema: "public",
                table: "ColumnMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVirtualColumn",
                schema: "public",
                table: "ColumnMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyDisplayColumnReferenceId",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "IsForeignKeyDisplayColumn",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "IsVirtualColumn",
                schema: "public",
                table: "ColumnMetadata");
        }
    }
}
