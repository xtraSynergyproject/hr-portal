using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210506_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                schema: "log",
                table: "ServiceIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                schema: "public",
                table: "ServiceIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnName",
                schema: "log",
                table: "ServiceIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "ColumnName",
                schema: "public",
                table: "ServiceIndexPageColumn");
        }
    }
}
