using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20211111_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableTwoFactorAuth",
                schema: "log",
                table: "UserLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableTwoFactorAuth",
                schema: "public",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableTwoFactorAuth",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "EnableTwoFactorAuth",
                schema: "public",
                table: "User");
        }
    }
}
