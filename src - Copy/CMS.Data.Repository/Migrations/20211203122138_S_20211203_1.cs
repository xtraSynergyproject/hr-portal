using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20211203_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TwoFactorAuthType",
                schema: "log",
                table: "UserLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TwoFactorAuthType",
                schema: "public",
                table: "User",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorAuthType",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "TwoFactorAuthType",
                schema: "public",
                table: "User");
        }
    }
}
