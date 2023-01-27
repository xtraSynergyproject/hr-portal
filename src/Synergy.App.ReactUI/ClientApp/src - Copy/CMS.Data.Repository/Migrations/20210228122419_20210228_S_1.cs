using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210228_S_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreateReturnType",
                schema: "public",
                table: "NoteTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DisableVersioning",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EditReturnType",
                schema: "public",
                table: "NoteTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SaveNewVersionButtonCss",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SaveNewVersionButtonText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NoteIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate",
                column: "OrderByColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate",
                column: "OrderByColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropIndex(
                name: "IX_NoteIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "CreateReturnType",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "DisableVersioning",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "EditReturnType",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "SaveNewVersionButtonCss",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "SaveNewVersionButtonText",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                schema: "public",
                table: "NoteIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "OrderByColumnId",
                schema: "public",
                table: "NoteIndexPageTemplate");
        }
    }
}
