using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20221116_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisableSubmitButton",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableSubmitButton",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                schema: "log",
                table: "CaptchaLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                schema: "public",
                table: "Captcha",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisableSubmitButton",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DisableSubmitButton",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                schema: "log",
                table: "CaptchaLog");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                schema: "public",
                table: "Captcha");
        }
    }
}
