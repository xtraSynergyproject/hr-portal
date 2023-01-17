using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210908_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionStatus",
                schema: "log",
                table: "NotificationTemplateLog",
                newName: "ActionStatusCodes");

            migrationBuilder.RenameColumn(
                name: "ActionStatus",
                schema: "public",
                table: "NotificationTemplate",
                newName: "ActionStatusCodes");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLog_NotificationTemplateId",
                schema: "log",
                table: "NotificationLog",
                column: "NotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationTemplateId",
                schema: "public",
                table: "Notification",
                column: "NotificationTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationTemplate_NotificationTemplateId",
                schema: "public",
                table: "Notification",
                column: "NotificationTemplateId",
                principalSchema: "public",
                principalTable: "NotificationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationLog_NotificationTemplate_NotificationTemplateId",
                schema: "log",
                table: "NotificationLog",
                column: "NotificationTemplateId",
                principalSchema: "public",
                principalTable: "NotificationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationTemplate_NotificationTemplateId",
                schema: "public",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationLog_NotificationTemplate_NotificationTemplateId",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropIndex(
                name: "IX_NotificationLog_NotificationTemplateId",
                schema: "log",
                table: "NotificationLog");

            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationTemplateId",
                schema: "public",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "ActionStatusCodes",
                schema: "log",
                table: "NotificationTemplateLog",
                newName: "ActionStatus");

            migrationBuilder.RenameColumn(
                name: "ActionStatusCodes",
                schema: "public",
                table: "NotificationTemplate",
                newName: "ActionStatus");
        }
    }
}
