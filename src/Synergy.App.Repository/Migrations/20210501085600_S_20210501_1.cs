using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210501_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyLog_Company_Id",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonEmail",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonMobile",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "log",
                table: "CompanyLog",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CultureInfo",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbol",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "DataAction",
                schema: "log",
                table: "CompanyLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DateFormat",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DateTimeFormat",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultEmailTemplate",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "log",
                table: "CompanyLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                schema: "log",
                table: "CompanyLog",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LicenseKey",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LogoFileId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NameLocal",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactEmail",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactMobile",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactPerson",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactPhone",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactEmail",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactMobile",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactPerson",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactPhone",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "SendCompanyWelcome",
                schema: "log",
                table: "CompanyLog",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SequenceOrder",
                schema: "log",
                table: "CompanyLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsGateway",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmsPassword",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmsSenderName",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmsUserId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmtpFromId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmtpHost",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmtpPassword",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "SmtpPort",
                schema: "log",
                table: "CompanyLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmtpSenderName",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SmtpUserId",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "log",
                table: "CompanyLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                schema: "log",
                table: "CompanyLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<long>(
                name: "VersionNo",
                schema: "log",
                table: "CompanyLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "ContactPersonEmail",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "ContactPersonMobile",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "CultureInfo",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "CurrencyName",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "CurrencySymbol",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "DataAction",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "DateFormat",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "DateTimeFormat",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "DefaultEmailTemplate",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "Fax",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LicenseKey",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "LogoFileId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "NameLocal",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "PrimaryContactEmail",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "PrimaryContactMobile",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "PrimaryContactPerson",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "PrimaryContactPhone",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SecondaryContactEmail",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SecondaryContactMobile",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SecondaryContactPerson",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SecondaryContactPhone",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SendCompanyWelcome",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SequenceOrder",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmsGateway",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmsPassword",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmsSenderName",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmsUserId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmtpFromId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmtpHost",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmtpPassword",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmtpPort",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmtpSenderName",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "SmtpUserId",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.DropColumn(
                name: "VersionNo",
                schema: "log",
                table: "CompanyLog");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyLog_Company_Id",
                schema: "log",
                table: "CompanyLog",
                column: "Id",
                principalSchema: "public",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
