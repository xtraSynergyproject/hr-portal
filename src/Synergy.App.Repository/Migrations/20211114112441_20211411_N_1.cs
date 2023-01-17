using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20211411_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OTPExpiry",
                schema: "log",
                table: "UserLog",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorAuthOTP",
                schema: "log",
                table: "UserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "OTPExpiry",
                schema: "public",
                table: "User",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorAuthOTP",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTPExpiry",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "TwoFactorAuthOTP",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "OTPExpiry",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TwoFactorAuthOTP",
                schema: "public",
                table: "User");
        }
    }
}
