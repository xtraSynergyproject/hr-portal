using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210604_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultDisplayColumnId",
                schema: "log",
                table: "TableMetadataLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultDisplayColumnId",
                schema: "public",
                table: "TableMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultDisplayColumnId",
                schema: "log",
                table: "TableMetadataLog");

            migrationBuilder.DropColumn(
                name: "DefaultDisplayColumnId",
                schema: "public",
                table: "TableMetadata");
        }
    }
}
