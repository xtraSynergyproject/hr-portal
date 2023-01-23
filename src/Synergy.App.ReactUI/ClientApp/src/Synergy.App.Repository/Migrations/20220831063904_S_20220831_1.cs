using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220831_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "log",
                table: "QRCodeDataLog",
                newName: "QrCodeUrl");

            migrationBuilder.RenameColumn(
                name: "TargetUrl",
                schema: "log",
                table: "QRCodeDataLog",
                newName: "QRCodeImageId");

            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "public",
                table: "QRCodeData",
                newName: "QrCodeUrl");

            migrationBuilder.RenameColumn(
                name: "TargetUrl",
                schema: "public",
                table: "QRCodeData",
                newName: "QRCodeImageId");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                schema: "log",
                table: "QRCodeDataLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsPopup",
                schema: "log",
                table: "QRCodeDataLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QRCodeDataType",
                schema: "log",
                table: "QRCodeDataLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QRCodeType",
                schema: "log",
                table: "QRCodeDataLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Data",
                schema: "public",
                table: "QRCodeData",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsPopup",
                schema: "public",
                table: "QRCodeData",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QRCodeDataType",
                schema: "public",
                table: "QRCodeData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QRCodeType",
                schema: "public",
                table: "QRCodeData",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                schema: "log",
                table: "QRCodeDataLog");

            migrationBuilder.DropColumn(
                name: "IsPopup",
                schema: "log",
                table: "QRCodeDataLog");

            migrationBuilder.DropColumn(
                name: "QRCodeDataType",
                schema: "log",
                table: "QRCodeDataLog");

            migrationBuilder.DropColumn(
                name: "QRCodeType",
                schema: "log",
                table: "QRCodeDataLog");

            migrationBuilder.DropColumn(
                name: "Data",
                schema: "public",
                table: "QRCodeData");

            migrationBuilder.DropColumn(
                name: "IsPopup",
                schema: "public",
                table: "QRCodeData");

            migrationBuilder.DropColumn(
                name: "QRCodeDataType",
                schema: "public",
                table: "QRCodeData");

            migrationBuilder.DropColumn(
                name: "QRCodeType",
                schema: "public",
                table: "QRCodeData");

            migrationBuilder.RenameColumn(
                name: "QrCodeUrl",
                schema: "log",
                table: "QRCodeDataLog",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "QRCodeImageId",
                schema: "log",
                table: "QRCodeDataLog",
                newName: "TargetUrl");

            migrationBuilder.RenameColumn(
                name: "QrCodeUrl",
                schema: "public",
                table: "QRCodeData",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "QRCodeImageId",
                schema: "public",
                table: "QRCodeData",
                newName: "TargetUrl");
        }
    }
}
