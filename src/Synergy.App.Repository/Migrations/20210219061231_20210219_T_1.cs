using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210219_T_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay10",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay6",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay7",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay8",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay9",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue10",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue6",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue7",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue8",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue9",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay10",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay6",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay7",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay8",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay9",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue10",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue6",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue7",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue8",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue9",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropdownDisplay10",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay6",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay7",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay8",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay9",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue10",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue6",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue7",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue8",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue9",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay10",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay6",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay7",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay8",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay9",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue10",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue6",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue7",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue8",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue9",
                schema: "public",
                table: "NtsTask");
        }
    }
}
