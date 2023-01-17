using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221117_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualImages",
                schema: "log",
                table: "CaptchaLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayImages",
                schema: "log",
                table: "CaptchaLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ActualImages",
                schema: "public",
                table: "Captcha",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplayImages",
                schema: "public",
                table: "Captcha",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualImages",
                schema: "log",
                table: "CaptchaLog");

            migrationBuilder.DropColumn(
                name: "DisplayImages",
                schema: "log",
                table: "CaptchaLog");

            migrationBuilder.DropColumn(
                name: "ActualImages",
                schema: "public",
                table: "Captcha");

            migrationBuilder.DropColumn(
                name: "DisplayImages",
                schema: "public",
                table: "Captcha");
        }
    }
}
