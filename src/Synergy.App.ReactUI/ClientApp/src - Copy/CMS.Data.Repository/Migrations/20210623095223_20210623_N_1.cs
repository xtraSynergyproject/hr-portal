using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210623_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEntityPermission_User_UserId",
                schema: "public",
                table: "UserEntityPermission");

            migrationBuilder.DropIndex(
                name: "IX_UserEntityPermission_UserId",
                schema: "public",
                table: "UserEntityPermission");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "public",
                table: "UserEntityPermission",
                newName: "UserEntityId");

            migrationBuilder.AddColumn<int>(
                name: "UserEntityType",
                schema: "public",
                table: "UserEntityPermission",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DataPermissionColumnId",
                schema: "log",
                table: "TaskTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableDataPermission",
                schema: "log",
                table: "TaskTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DataPermissionColumnId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableDataPermission",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DataPermissionColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableDataPermission",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DataPermissionColumnId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableDataPermission",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DataPermissionColumnId",
                schema: "log",
                table: "NoteTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableDataPermission",
                schema: "log",
                table: "NoteTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DataPermissionColumnId",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnableDataPermission",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEntityType",
                schema: "public",
                table: "UserEntityPermission");

            migrationBuilder.DropColumn(
                name: "DataPermissionColumnId",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableDataPermission",
                schema: "log",
                table: "TaskTemplateLog");

            migrationBuilder.DropColumn(
                name: "DataPermissionColumnId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "EnableDataPermission",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DataPermissionColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableDataPermission",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DataPermissionColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EnableDataPermission",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DataPermissionColumnId",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableDataPermission",
                schema: "log",
                table: "NoteTemplateLog");

            migrationBuilder.DropColumn(
                name: "DataPermissionColumnId",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "EnableDataPermission",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.RenameColumn(
                name: "UserEntityId",
                schema: "public",
                table: "UserEntityPermission",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntityPermission_UserId",
                schema: "public",
                table: "UserEntityPermission",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntityPermission_User_UserId",
                schema: "public",
                table: "UserEntityPermission",
                column: "UserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
