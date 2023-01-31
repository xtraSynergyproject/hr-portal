using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210414_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UdfNoteTableMetadataId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfNoteTemplateId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Template_UdfNoteTableMetadataId",
                schema: "public",
                table: "Template",
                column: "UdfNoteTableMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_UdfNoteTemplateId",
                schema: "public",
                table: "Template",
                column: "UdfNoteTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_NoteTemplate_UdfNoteTemplateId",
                schema: "public",
                table: "Template",
                column: "UdfNoteTemplateId",
                principalSchema: "public",
                principalTable: "NoteTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_TableMetadata_UdfNoteTableMetadataId",
                schema: "public",
                table: "Template",
                column: "UdfNoteTableMetadataId",
                principalSchema: "public",
                principalTable: "TableMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_NoteTemplate_UdfNoteTemplateId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_TableMetadata_UdfNoteTableMetadataId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Template_UdfNoteTableMetadataId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Template_UdfNoteTemplateId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "UdfNoteTableMetadataId",
                schema: "public",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "UdfNoteTemplateId",
                schema: "public",
                table: "Template");
        }
    }
}
