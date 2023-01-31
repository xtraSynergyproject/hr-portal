using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210000000515_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStatus",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "UserStatus",
                schema: "public",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserStatus",
                schema: "log",
                table: "UserLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserStatus",
                schema: "public",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
