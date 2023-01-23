using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210211_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CultureInfo",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbol",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DateFormat",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DateTimeFormat",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultEmailTemplate",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LicenseKey",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameLocal",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactEmail",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactMobile",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactPerson",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactPhone",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactEmail",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactMobile",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactPerson",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactPhone",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "SendCompanyWelcome",
                schema: "public",
                table: "Company",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsGateway",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmsPassword",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmsSenderName",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmsUserId",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmtpFromId",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmtpHost",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmtpPassword",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "SmtpPort",
                schema: "public",
                table: "Company",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmtpSenderName",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmtpUserId",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                schema: "public",
                table: "Company",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CultureInfo",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CurrencyName",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CurrencySymbol",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "DateFormat",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "DateTimeFormat",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "DefaultEmailTemplate",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LicenseKey",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "NameLocal",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PrimaryContactEmail",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PrimaryContactMobile",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PrimaryContactPerson",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PrimaryContactPhone",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SecondaryContactEmail",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SecondaryContactMobile",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SecondaryContactPerson",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SecondaryContactPhone",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SendCompanyWelcome",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmsGateway",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmsPassword",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmsSenderName",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmsUserId",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmtpFromId",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmtpHost",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmtpPassword",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmtpPort",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmtpSenderName",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmtpUserId",
                schema: "public",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                schema: "public",
                table: "Company");
        }
    }
}
