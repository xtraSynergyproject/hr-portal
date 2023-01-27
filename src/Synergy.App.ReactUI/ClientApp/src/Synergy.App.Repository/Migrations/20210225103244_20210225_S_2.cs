using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210225_S_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormTemplate_IndexPageTemplate_IndexPageTemplateId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_IndexPageColumn_ColumnMetadata_ColumnMetadataId",
                schema: "public",
                table: "IndexPageColumn");

            migrationBuilder.DropForeignKey(
                name: "FK_IndexPageColumn_IndexPageTemplate_FormIndexPageTemplateId",
                schema: "public",
                table: "IndexPageColumn");

            migrationBuilder.DropForeignKey(
                name: "FK_IndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_IndexPageTemplate_Template_TemplateId",
                schema: "public",
                table: "IndexPageTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IndexPageTemplate",
                schema: "public",
                table: "IndexPageTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IndexPageColumn",
                schema: "public",
                table: "IndexPageColumn");

            migrationBuilder.RenameTable(
                name: "IndexPageTemplate",
                schema: "public",
                newName: "FormIndexPageTemplate",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "IndexPageColumn",
                schema: "public",
                newName: "FormIndexPageColumn",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_IndexPageTemplate_TemplateId",
                schema: "public",
                table: "FormIndexPageTemplate",
                newName: "IX_FormIndexPageTemplate_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_IndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "FormIndexPageTemplate",
                newName: "IX_FormIndexPageTemplate_OrderByColumnId");

            migrationBuilder.RenameIndex(
                name: "IX_IndexPageColumn_FormIndexPageTemplateId",
                schema: "public",
                table: "FormIndexPageColumn",
                newName: "IX_FormIndexPageColumn_FormIndexPageTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_IndexPageColumn_ColumnMetadataId",
                schema: "public",
                table: "FormIndexPageColumn",
                newName: "IX_FormIndexPageColumn_ColumnMetadataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormIndexPageTemplate",
                schema: "public",
                table: "FormIndexPageTemplate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormIndexPageColumn",
                schema: "public",
                table: "FormIndexPageColumn",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormIndexPageColumn_ColumnMetadata_ColumnMetadataId",
                schema: "public",
                table: "FormIndexPageColumn",
                column: "ColumnMetadataId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormIndexPageColumn_FormIndexPageTemplate_FormIndexPageTemp~",
                schema: "public",
                table: "FormIndexPageColumn",
                column: "FormIndexPageTemplateId",
                principalSchema: "public",
                principalTable: "FormIndexPageTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "FormIndexPageTemplate",
                column: "OrderByColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormIndexPageTemplate_Template_TemplateId",
                schema: "public",
                table: "FormIndexPageTemplate",
                column: "TemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormTemplate_FormIndexPageTemplate_IndexPageTemplateId",
                schema: "public",
                table: "FormTemplate",
                column: "IndexPageTemplateId",
                principalSchema: "public",
                principalTable: "FormIndexPageTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormIndexPageColumn_ColumnMetadata_ColumnMetadataId",
                schema: "public",
                table: "FormIndexPageColumn");

            migrationBuilder.DropForeignKey(
                name: "FK_FormIndexPageColumn_FormIndexPageTemplate_FormIndexPageTemp~",
                schema: "public",
                table: "FormIndexPageColumn");

            migrationBuilder.DropForeignKey(
                name: "FK_FormIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "FormIndexPageTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FormIndexPageTemplate_Template_TemplateId",
                schema: "public",
                table: "FormIndexPageTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FormTemplate_FormIndexPageTemplate_IndexPageTemplateId",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormIndexPageTemplate",
                schema: "public",
                table: "FormIndexPageTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormIndexPageColumn",
                schema: "public",
                table: "FormIndexPageColumn");

            migrationBuilder.RenameTable(
                name: "FormIndexPageTemplate",
                schema: "public",
                newName: "IndexPageTemplate",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "FormIndexPageColumn",
                schema: "public",
                newName: "IndexPageColumn",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_FormIndexPageTemplate_TemplateId",
                schema: "public",
                table: "IndexPageTemplate",
                newName: "IX_IndexPageTemplate_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_FormIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate",
                newName: "IX_IndexPageTemplate_OrderByColumnId");

            migrationBuilder.RenameIndex(
                name: "IX_FormIndexPageColumn_FormIndexPageTemplateId",
                schema: "public",
                table: "IndexPageColumn",
                newName: "IX_IndexPageColumn_FormIndexPageTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_FormIndexPageColumn_ColumnMetadataId",
                schema: "public",
                table: "IndexPageColumn",
                newName: "IX_IndexPageColumn_ColumnMetadataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndexPageTemplate",
                schema: "public",
                table: "IndexPageTemplate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndexPageColumn",
                schema: "public",
                table: "IndexPageColumn",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormTemplate_IndexPageTemplate_IndexPageTemplateId",
                schema: "public",
                table: "FormTemplate",
                column: "IndexPageTemplateId",
                principalSchema: "public",
                principalTable: "IndexPageTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndexPageColumn_ColumnMetadata_ColumnMetadataId",
                schema: "public",
                table: "IndexPageColumn",
                column: "ColumnMetadataId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndexPageColumn_IndexPageTemplate_FormIndexPageTemplateId",
                schema: "public",
                table: "IndexPageColumn",
                column: "FormIndexPageTemplateId",
                principalSchema: "public",
                principalTable: "IndexPageTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate",
                column: "OrderByColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndexPageTemplate_Template_TemplateId",
                schema: "public",
                table: "IndexPageTemplate",
                column: "TemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
