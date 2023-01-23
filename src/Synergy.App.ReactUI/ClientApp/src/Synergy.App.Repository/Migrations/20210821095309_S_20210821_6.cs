using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210821_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UdfPermissionHeader",
                schema: "log",
                table: "UdfPermissionLog");

            migrationBuilder.DropColumn(
                name: "UdfPermissionHeader",
                schema: "public",
                table: "UdfPermission");

            migrationBuilder.AddColumn<string[]>(
                name: "CategoryCodes",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NtsType",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string[]>(
                name: "TemplateCodes",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "CategoryCodes",
                schema: "public",
                table: "UdfPermissionHeader",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NtsType",
                schema: "public",
                table: "UdfPermissionHeader",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string[]>(
                name: "TemplateCodes",
                schema: "public",
                table: "UdfPermissionHeader",
                type: "text[]",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "CategoryCodes",
                schema: "log",
                table: "UdfPermissionHeaderLog");

            migrationBuilder.DropColumn(
                name: "NtsType",
                schema: "log",
                table: "UdfPermissionHeaderLog");

            migrationBuilder.DropColumn(
                name: "TemplateCodes",
                schema: "log",
                table: "UdfPermissionHeaderLog");

            migrationBuilder.DropColumn(
                name: "CategoryCodes",
                schema: "public",
                table: "UdfPermissionHeader");

            migrationBuilder.DropColumn(
                name: "NtsType",
                schema: "public",
                table: "UdfPermissionHeader");

            migrationBuilder.DropColumn(
                name: "TemplateCodes",
                schema: "public",
                table: "UdfPermissionHeader");

            migrationBuilder.AddColumn<string>(
                name: "UdfPermissionHeader",
                schema: "log",
                table: "UdfPermissionLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfPermissionHeader",
                schema: "public",
                table: "UdfPermission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
