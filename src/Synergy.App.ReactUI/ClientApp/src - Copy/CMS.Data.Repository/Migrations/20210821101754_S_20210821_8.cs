using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210821_8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryTemplateId",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryTemplateId",
                schema: "public",
                table: "UdfPermissionHeader",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionHeaderLog_PrimaryTemplateId",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                column: "PrimaryTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionHeader_PrimaryTemplateId",
                schema: "public",
                table: "UdfPermissionHeader",
                column: "PrimaryTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_UdfPermissionHeader_Template_PrimaryTemplateId",
                schema: "public",
                table: "UdfPermissionHeader",
                column: "PrimaryTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UdfPermissionHeaderLog_Template_PrimaryTemplateId",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                column: "PrimaryTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UdfPermissionHeader_Template_PrimaryTemplateId",
                schema: "public",
                table: "UdfPermissionHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_UdfPermissionHeaderLog_Template_PrimaryTemplateId",
                schema: "log",
                table: "UdfPermissionHeaderLog");

            migrationBuilder.DropIndex(
                name: "IX_UdfPermissionHeaderLog_PrimaryTemplateId",
                schema: "log",
                table: "UdfPermissionHeaderLog");

            migrationBuilder.DropIndex(
                name: "IX_UdfPermissionHeader_PrimaryTemplateId",
                schema: "public",
                table: "UdfPermissionHeader");

            migrationBuilder.DropColumn(
                name: "PrimaryTemplateId",
                schema: "log",
                table: "UdfPermissionHeaderLog");

            migrationBuilder.DropColumn(
                name: "PrimaryTemplateId",
                schema: "public",
                table: "UdfPermissionHeader");
        }
    }
}
