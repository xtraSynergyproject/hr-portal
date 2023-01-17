using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210903_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentNoteId",
                schema: "log",
                table: "NtsTaskLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentNoteId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentNoteId",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentTaskId",
                schema: "log",
                table: "NtsServiceLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentNoteId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ParentTaskId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentNoteId",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "ParentNoteId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ParentNoteId",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "ParentTaskId",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "ParentNoteId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ParentTaskId",
                schema: "public",
                table: "NtsService");
        }
    }
}
