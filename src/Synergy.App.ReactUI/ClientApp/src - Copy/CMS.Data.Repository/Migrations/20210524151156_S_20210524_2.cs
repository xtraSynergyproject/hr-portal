using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210524_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DomainId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubDomainId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DomainId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubDomainId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateLog_DomainId",
                schema: "log",
                table: "TemplateLog",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateLog_SubDomainId",
                schema: "log",
                table: "TemplateLog",
                column: "SubDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_DomainId",
                schema: "public",
                table: "Template",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_SubDomainId",
                schema: "public",
                table: "Template",
                column: "SubDomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_LOV_DomainId",
                schema: "public",
                table: "Template",
                column: "DomainId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_LOV_SubDomainId",
                schema: "public",
                table: "Template",
                column: "SubDomainId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateLog_LOV_DomainId",
                schema: "log",
                table: "TemplateLog",
                column: "DomainId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateLog_LOV_SubDomainId",
                schema: "log",
                table: "TemplateLog",
                column: "SubDomainId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_LOV_DomainId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_LOV_SubDomainId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateLog_LOV_DomainId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateLog_LOV_SubDomainId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TemplateLog_DomainId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_TemplateLog_SubDomainId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_Template_DomainId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Template_SubDomainId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "DomainId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "SubDomainId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "DomainId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "SubDomainId",
                schema: "public",
                table: "Template");
        }
    }
}
