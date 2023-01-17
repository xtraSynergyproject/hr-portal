using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20212911_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MongoPreviewFileId",
                schema: "log",
                table: "FileLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "MongoPreviewFileId",
                schema: "public",
                table: "File",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MongoPreviewFileId",
                schema: "log",
                table: "FileLog");

            migrationBuilder.DropColumn(
                name: "MongoPreviewFileId",
                schema: "public",
                table: "File");
        }
    }
}
