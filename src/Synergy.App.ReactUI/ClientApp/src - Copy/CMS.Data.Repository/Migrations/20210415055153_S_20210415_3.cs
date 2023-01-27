using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210415_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UdfTableMetadataId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Template_UdfTableMetadataId",
                schema: "public",
                table: "Template",
                column: "UdfTableMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_UdfTemplateId",
                schema: "public",
                table: "Template",
                column: "UdfTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_TableMetadata_UdfTableMetadataId",
                schema: "public",
                table: "Template",
                column: "UdfTableMetadataId",
                principalSchema: "public",
                principalTable: "TableMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_Template_UdfTemplateId",
                schema: "public",
                table: "Template",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_TableMetadata_UdfTableMetadataId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_Template_UdfTemplateId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Template_UdfTableMetadataId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Template_UdfTemplateId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "UdfTableMetadataId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "public",
                table: "Template");
        }
    }
}
