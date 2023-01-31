using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210709_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentTableMetadata",
                schema: "log",
                table: "TableMetadataLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentTableMetadata",
                schema: "public",
                table: "TableMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentTableMetadata",
                schema: "log",
                table: "TableMetadataLog");

            migrationBuilder.DropColumn(
                name: "ParentTableMetadata",
                schema: "public",
                table: "TableMetadata");
        }
    }
}
