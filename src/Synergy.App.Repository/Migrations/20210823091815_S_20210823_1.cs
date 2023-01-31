using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210823_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UdfPermission_UdfPermissionHeader_UdfPermissionHeaderId",
                schema: "public",
                table: "UdfPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_UdfPermissionLog_UdfPermissionHeader_UdfPermissionHeaderId",
                schema: "log",
                table: "UdfPermissionLog");

            migrationBuilder.DropIndex(
                name: "IX_UdfPermissionLog_UdfPermissionHeaderId",
                schema: "log",
                table: "UdfPermissionLog");

            migrationBuilder.DropIndex(
                name: "IX_UdfPermission_UdfPermissionHeaderId",
                schema: "public",
                table: "UdfPermission");

            migrationBuilder.DropColumn(
                name: "UdfPermissionHeaderId",
                schema: "log",
                table: "UdfPermissionLog");

            migrationBuilder.DropColumn(
                name: "UdfPermissionHeaderId",
                schema: "public",
                table: "UdfPermission");

            migrationBuilder.AddColumn<bool>(
                name: "EnableInlineComment",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePermission",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableInlineComment",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePermission",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableAdhocTask",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableInlineComment",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePermission",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableAdhocTask",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableInlineComment",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePermission",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UdfPermissionHeaderId",
                schema: "log",
                table: "ServiceIndexPageColumnLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfPermissionHeaderId",
                schema: "public",
                table: "ServiceIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableInlineComment",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableInlineComment",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceIndexPageColumnLog_UdfPermissionHeaderId",
                schema: "log",
                table: "ServiceIndexPageColumnLog",
                column: "UdfPermissionHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceIndexPageColumn_UdfPermissionHeaderId",
                schema: "public",
                table: "ServiceIndexPageColumn",
                column: "UdfPermissionHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceIndexPageColumn_UdfPermissionHeader_UdfPermissionHea~",
                schema: "public",
                table: "ServiceIndexPageColumn",
                column: "UdfPermissionHeaderId",
                principalSchema: "public",
                principalTable: "UdfPermissionHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceIndexPageColumnLog_UdfPermissionHeader_UdfPermission~",
                schema: "log",
                table: "ServiceIndexPageColumnLog",
                column: "UdfPermissionHeaderId",
                principalSchema: "public",
                principalTable: "UdfPermissionHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceIndexPageColumn_UdfPermissionHeader_UdfPermissionHea~",
                schema: "public",
                table: "ServiceIndexPageColumn");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceIndexPageColumnLog_UdfPermissionHeader_UdfPermission~",
                schema: "log",
                table: "ServiceIndexPageColumnLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceIndexPageColumnLog_UdfPermissionHeaderId",
                schema: "log",
                table: "ServiceIndexPageColumnLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceIndexPageColumn_UdfPermissionHeaderId",
                schema: "public",
                table: "ServiceIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "EnableInlineComment",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnablePermission",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableInlineComment",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnablePermission",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableAdhocTask",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableInlineComment",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnablePermission",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableAdhocTask",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EnableInlineComment",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EnablePermission",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "UdfPermissionHeaderId",
                schema: "log",
                table: "ServiceIndexPageColumnLog");

            migrationBuilder.DropColumn(
                name: "UdfPermissionHeaderId",
                schema: "public",
                table: "ServiceIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "EnableInlineComment",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableInlineComment",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.AddColumn<string>(
                name: "UdfPermissionHeaderId",
                schema: "log",
                table: "UdfPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfPermissionHeaderId",
                schema: "public",
                table: "UdfPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionLog_UdfPermissionHeaderId",
                schema: "log",
                table: "UdfPermissionLog",
                column: "UdfPermissionHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermission_UdfPermissionHeaderId",
                schema: "public",
                table: "UdfPermission",
                column: "UdfPermissionHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_UdfPermission_UdfPermissionHeader_UdfPermissionHeaderId",
                schema: "public",
                table: "UdfPermission",
                column: "UdfPermissionHeaderId",
                principalSchema: "public",
                principalTable: "UdfPermissionHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UdfPermissionLog_UdfPermissionHeader_UdfPermissionHeaderId",
                schema: "log",
                table: "UdfPermissionLog",
                column: "UdfPermissionHeaderId",
                principalSchema: "public",
                principalTable: "UdfPermissionHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
