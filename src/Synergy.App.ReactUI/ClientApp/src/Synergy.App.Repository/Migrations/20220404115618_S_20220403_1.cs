using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220403_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CityName",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "GSTNo",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PANNo",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TANNo",
                schema: "log",
                table: "LegalEntityLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "GSTNo",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PANNo",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TANNo",
                schema: "public",
                table: "LegalEntity",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityName",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "GSTNo",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "MobileNo",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "PANNo",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "PinCode",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "StateName",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "TANNo",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "CityName",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "GSTNo",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "MobileNo",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "PANNo",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "PinCode",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "StateName",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "TANNo",
                schema: "public",
                table: "LegalEntity");
        }
    }
}
