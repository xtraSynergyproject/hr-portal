using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220622_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.RenameColumn(
                name: "LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "PostSubmitPageParams");

            migrationBuilder.RenameColumn(
                name: "LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate",
                newName: "PostSubmitPageParams");

            migrationBuilder.AddColumn<bool>(
                name: "EnablePostSubmitPage",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PostSubmitPageAction",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PostSubmitPageArea",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PostSubmitPageController",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "EnablePostSubmitPage",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PostSubmitPageAction",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PostSubmitPageArea",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PostSubmitPageController",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnablePostSubmitPage",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "PostSubmitPageAction",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "PostSubmitPageArea",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "PostSubmitPageController",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnablePostSubmitPage",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "PostSubmitPageAction",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "PostSubmitPageArea",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "PostSubmitPageController",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.RenameColumn(
                name: "PostSubmitPageParams",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "LocalizedColumnId");

            migrationBuilder.RenameColumn(
                name: "PostSubmitPageParams",
                schema: "public",
                table: "ServiceTemplate",
                newName: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "LocalizedColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate",
                column: "LocalizedColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_ColumnMetadata_LocalizedColumnId",
                schema: "public",
                table: "ServiceTemplate",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_ColumnMetadata_LocalizedColumnId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "LocalizedColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
