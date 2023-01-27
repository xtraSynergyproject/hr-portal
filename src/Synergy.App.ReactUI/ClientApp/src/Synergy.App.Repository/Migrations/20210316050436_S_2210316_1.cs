using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210316_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                schema: "public",
                table: "TaskIndexPageTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderByColumnId",
                schema: "public",
                table: "TaskIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderByColumnId",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TaskIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "TaskIndexPageTemplate",
                column: "OrderByColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                column: "OrderByColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "ServiceIndexPageTemplate",
                column: "OrderByColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "TaskIndexPageTemplate",
                column: "OrderByColumnId",
                principalSchema: "public",
                principalTable: "ColumnMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskIndexPageTemplate_ColumnMetadata_OrderByColumnId",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TaskIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceIndexPageTemplate_OrderByColumnId",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "OrderByColumnId",
                schema: "public",
                table: "TaskIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                schema: "public",
                table: "ServiceIndexPageTemplate");

            migrationBuilder.DropColumn(
                name: "OrderByColumnId",
                schema: "public",
                table: "ServiceIndexPageTemplate");
        }
    }
}
