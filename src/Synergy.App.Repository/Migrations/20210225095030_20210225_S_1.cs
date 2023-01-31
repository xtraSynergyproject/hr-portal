using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210225_S_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                schema: "public",
                table: "IndexPageTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "CreateReturnType",
                schema: "public",
                table: "FormTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EditReturnType",
                schema: "public",
                table: "FormTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate",
                column: "OrderByColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate",
                column: "OrderByColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate");

            migrationBuilder.DropIndex(
                name: "IX_IndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                schema: "public",
                table: "IndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "OrderByColumnId",
                schema: "public",
                table: "IndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "CreateReturnType",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.DropColumn(
                name: "EditReturnType",
                schema: "public",
                table: "FormTemplate");

            migrationBuilder.AddColumn<bool>(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);
        }
    }
}
