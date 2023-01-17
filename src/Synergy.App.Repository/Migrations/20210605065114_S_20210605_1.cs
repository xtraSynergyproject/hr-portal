using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210605_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayFormat",
                schema: "log",
                table: "TaskIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayFormat",
                schema: "public",
                table: "TaskIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayFormat",
                schema: "log",
                table: "ServiceIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayFormat",
                schema: "public",
                table: "ServiceIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayFormat",
                schema: "log",
                table: "NoteIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayFormat",
                schema: "public",
                table: "NoteIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayFormat",
                schema: "log",
                table: "FormIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayFormat",
                schema: "public",
                table: "FormIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayFormat",
                schema: "log",
                table: "TaskIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "DisplayFormat",
                schema: "public",
                table: "TaskIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "DisplayFormat",
                schema: "log",
                table: "ServiceIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "DisplayFormat",
                schema: "public",
                table: "ServiceIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "DisplayFormat",
                schema: "log",
                table: "NoteIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "DisplayFormat",
                schema: "public",
                table: "NoteIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "DisplayFormat",
                schema: "log",
                table: "FormIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "DisplayFormat",
                schema: "public",
                table: "FormIndexPageColumn");
        }
    }
}
