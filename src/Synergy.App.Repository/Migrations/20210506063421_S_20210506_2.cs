using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210506_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                schema: "log",
                table: "TaskIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                schema: "public",
                table: "TaskIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                schema: "log",
                table: "NoteIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                schema: "public",
                table: "NoteIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnName",
                schema: "log",
                table: "TaskIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "ColumnName",
                schema: "public",
                table: "TaskIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "ColumnName",
                schema: "log",
                table: "NoteIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "ColumnName",
                schema: "public",
                table: "NoteIndexPageColumn");
        }
    }
}
