using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210316_8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableExportToExcel",
                schema: "public",
                table: "TaskIndexPageTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableExportToPdf",
                schema: "public",
                table: "TaskIndexPageTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequestButtonText",
                schema: "public",
                table: "TaskIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableExportToExcel",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableExportToPdf",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequestButtonText",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableExportToExcel",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableExportToPdf",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequestButtonText",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableExportToExcel",
                schema: "public",
                table: "FormIndexPageTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableExportToPdf",
                schema: "public",
                table: "FormIndexPageTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableExportToExcel",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "EnableExportToPdf",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "RequestButtonText",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "EnableExportToExcel",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "EnableExportToPdf",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "RequestButtonText",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "EnableExportToExcel",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "EnableExportToPdf",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "RequestButtonText",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "EnableExportToExcel",
                schema: "public",
                table: "FormIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "EnableExportToPdf",
                schema: "public",
                table: "FormIndexPageTemplate");
        }
    }
}
