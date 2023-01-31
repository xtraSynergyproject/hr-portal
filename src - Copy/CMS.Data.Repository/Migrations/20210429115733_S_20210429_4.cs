using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210429_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationAction",
                schema: "public",
                table: "TaskNotificationTemplate");

            migrationBuilder.DropColumn(
                name: "NotificationAction",
                schema: "public",
                table: "ServiceNotificationTemplate");

            migrationBuilder.DropColumn(
                name: "NotificationAction",
                schema: "public",
                table: "NoteNotificationTemplate");

            migrationBuilder.AddColumn<string>(
                name: "NotificationActionId",
                schema: "public",
                table: "TaskNotificationTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NotificationActionId",
                schema: "public",
                table: "ServiceNotificationTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NotificationActionId",
                schema: "public",
                table: "NoteNotificationTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationActionId",
                schema: "public",
                table: "TaskNotificationTemplate");

            migrationBuilder.DropColumn(
                name: "NotificationActionId",
                schema: "public",
                table: "ServiceNotificationTemplate");

            migrationBuilder.DropColumn(
                name: "NotificationActionId",
                schema: "public",
                table: "NoteNotificationTemplate");

            migrationBuilder.AddColumn<int>(
                name: "NotificationAction",
                schema: "public",
                table: "TaskNotificationTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotificationAction",
                schema: "public",
                table: "ServiceNotificationTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotificationAction",
                schema: "public",
                table: "NoteNotificationTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
