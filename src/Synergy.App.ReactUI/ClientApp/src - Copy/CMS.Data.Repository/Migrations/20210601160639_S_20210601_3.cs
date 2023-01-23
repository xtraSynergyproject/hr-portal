using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210601_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UdfNoteTableId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfNoteTableId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UdfNoteTableId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "UdfNoteTableId",
                schema: "public",
                table: "NtsService");
        }
    }
}
