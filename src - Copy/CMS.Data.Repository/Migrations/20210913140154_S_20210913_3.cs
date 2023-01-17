using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210913_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                schema: "log",
                table: "CustomIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                schema: "public",
                table: "CustomIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnName",
                schema: "log",
                table: "CustomIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "ColumnName",
                schema: "public",
                table: "CustomIndexPageColumn");
        }
    }
}
