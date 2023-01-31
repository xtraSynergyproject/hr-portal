using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210311_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForeignKeyDisplayColumn",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "ColumnMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKeyDisplayColumn",
                schema: "public",
                table: "ColumnMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
