using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220904_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TriggeredByReferenceType",
                schema: "log",
                table: "NotificationLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TriggeredByReferenceTypeId",
                schema: "log",
                table: "NotificationLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "TriggeredByReferenceType",
                schema: "public",
                table: "Notification",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TriggeredByReferenceTypeId",
                schema: "public",
                table: "Notification",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggeredByReferenceType",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "TriggeredByReferenceTypeId",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "TriggeredByReferenceType",
                schema: "public",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "TriggeredByReferenceTypeId",
                schema: "public",
                table: "Notification");
        }
    }
}
