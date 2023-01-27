using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210416_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "public",
                table: "TaskIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplate_UdfTemplateId",
                schema: "public",
                table: "TaskTemplate",
                column: "UdfTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskIndexPageTemplate_UdfTemplateId",
                schema: "public",
                table: "TaskIndexPageTemplate",
                column: "UdfTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_UdfTemplateId",
                schema: "public",
                table: "ServiceTemplate",
                column: "UdfTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceIndexPageTemplate_UdfTemplateId",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                column: "UdfTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceIndexPageTemplate_Template_UdfTemplateId",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_Template_UdfTemplateId",
                schema: "public",
                table: "ServiceTemplate",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskIndexPageTemplate_Template_UdfTemplateId",
                schema: "public",
                table: "TaskIndexPageTemplate",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplate_Template_UdfTemplateId",
                schema: "public",
                table: "TaskTemplate",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceIndexPageTemplate_Template_UdfTemplateId",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_Template_UdfTemplateId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskIndexPageTemplate_Template_UdfTemplateId",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplate_Template_UdfTemplateId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplate_UdfTemplateId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TaskIndexPageTemplate_UdfTemplateId",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_UdfTemplateId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceIndexPageTemplate_UdfTemplateId",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "public",
                table: "ServiceIndexPageTemplate");
        }
    }
}
