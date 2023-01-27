using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220922_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisableCaptcha",
                schema: "log",
                table: "UserLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverrideCaptchaSettings",
                schema: "log",
                table: "UserLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverrideTwoFactorAuthentication",
                schema: "log",
                table: "UserLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableCaptcha",
                schema: "public",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverrideCaptchaSettings",
                schema: "public",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverrideTwoFactorAuthentication",
                schema: "public",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EscalatedToNotificationTemplateId",
                schema: "log",
                table: "StepTaskEscalationLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "EscalatedToNotificationTemplateId",
                schema: "public",
                table: "StepTaskEscalation",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "DisableCaptcha",
                schema: "log",
                table: "PortalLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableTwoFactorAuth",
                schema: "log",
                table: "PortalLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TwoFactorAuthType",
                schema: "log",
                table: "PortalLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisableCaptcha",
                schema: "public",
                table: "Portal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableTwoFactorAuth",
                schema: "public",
                table: "Portal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TwoFactorAuthType",
                schema: "public",
                table: "Portal",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalationLog_EscalatedToNotificationTemplateId",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "EscalatedToNotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskEscalation_EscalatedToNotificationTemplateId",
                schema: "public",
                table: "StepTaskEscalation",
                column: "EscalatedToNotificationTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskEscalation_NotificationTemplate_EscalatedToNotifica~",
                schema: "public",
                table: "StepTaskEscalation",
                column: "EscalatedToNotificationTemplateId",
                principalSchema: "public",
                principalTable: "NotificationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskEscalationLog_NotificationTemplate_EscalatedToNotif~",
                schema: "log",
                table: "StepTaskEscalationLog",
                column: "EscalatedToNotificationTemplateId",
                principalSchema: "public",
                principalTable: "NotificationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskEscalation_NotificationTemplate_EscalatedToNotifica~",
                schema: "public",
                table: "StepTaskEscalation");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskEscalationLog_NotificationTemplate_EscalatedToNotif~",
                schema: "log",
                table: "StepTaskEscalationLog");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskEscalationLog_EscalatedToNotificationTemplateId",
                schema: "log",
                table: "StepTaskEscalationLog");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskEscalation_EscalatedToNotificationTemplateId",
                schema: "public",
                table: "StepTaskEscalation");

            migrationBuilder.DropColumn(
                name: "DisableCaptcha",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "OverrideCaptchaSettings",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "OverrideTwoFactorAuthentication",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "DisableCaptcha",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "OverrideCaptchaSettings",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "OverrideTwoFactorAuthentication",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EscalatedToNotificationTemplateId",
                schema: "log",
                table: "StepTaskEscalationLog");

            migrationBuilder.DropColumn(
                name: "EscalatedToNotificationTemplateId",
                schema: "public",
                table: "StepTaskEscalation");

            migrationBuilder.DropColumn(
                name: "DisableCaptcha",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "EnableTwoFactorAuth",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "TwoFactorAuthType",
                schema: "log",
                table: "PortalLog");

            migrationBuilder.DropColumn(
                name: "DisableCaptcha",
                schema: "public",
                table: "Portal");

            migrationBuilder.DropColumn(
                name: "EnableTwoFactorAuth",
                schema: "public",
                table: "Portal");

            migrationBuilder.DropColumn(
                name: "TwoFactorAuthType",
                schema: "public",
                table: "Portal");
        }
    }
}
