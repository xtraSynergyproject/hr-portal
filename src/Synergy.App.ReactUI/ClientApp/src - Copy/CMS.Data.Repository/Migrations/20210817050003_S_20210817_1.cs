using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210817_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "log",
                table: "NotificationLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                schema: "log",
                table: "NotificationLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "public",
                table: "Notification",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                schema: "public",
                table: "Notification",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "public",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                schema: "public",
                table: "Notification");
        }
    }
}
