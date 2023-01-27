using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210908_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "ActionStatus",
                schema: "log",
                table: "NotificationTemplateLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                schema: "log",
                table: "NotificationTemplateLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string[]>(
                name: "ActionStatus",
                schema: "public",
                table: "NotificationTemplate",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                schema: "public",
                table: "NotificationTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionStatus",
                schema: "log",
                table: "NotificationLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NotificationTemplateId",
                schema: "log",
                table: "NotificationLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ActionStatus",
                schema: "public",
                table: "Notification",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NotificationTemplateId",
                schema: "public",
                table: "Notification",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionStatus",
                schema: "log",
                table: "NotificationTemplateLog");

            migrationBuilder.DropColumn(
                name: "ActionType",
                schema: "log",
                table: "NotificationTemplateLog");

            migrationBuilder.DropColumn(
                name: "ActionStatus",
                schema: "public",
                table: "NotificationTemplate");

            migrationBuilder.DropColumn(
                name: "ActionType",
                schema: "public",
                table: "NotificationTemplate");

            migrationBuilder.DropColumn(
                name: "ActionStatus",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "NotificationTemplateId",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "ActionStatus",
                schema: "public",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "NotificationTemplateId",
                schema: "public",
                table: "Notification");
        }
    }
}
