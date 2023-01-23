using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210416_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UdfNoteId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfNoteId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfTemplateId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_UdfNoteId",
                schema: "public",
                table: "NtsTask",
                column: "UdfNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_UdfTemplateId",
                schema: "public",
                table: "NtsTask",
                column: "UdfTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_UdfNoteId",
                schema: "public",
                table: "NtsService",
                column: "UdfNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_UdfTemplateId",
                schema: "public",
                table: "NtsService",
                column: "UdfTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_NtsNote_UdfNoteId",
                schema: "public",
                table: "NtsService",
                column: "UdfNoteId",
                principalSchema: "public",
                principalTable: "NtsNote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_Template_UdfTemplateId",
                schema: "public",
                table: "NtsService",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_NtsNote_UdfNoteId",
                schema: "public",
                table: "NtsTask",
                column: "UdfNoteId",
                principalSchema: "public",
                principalTable: "NtsNote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_Template_UdfTemplateId",
                schema: "public",
                table: "NtsTask",
                column: "UdfTemplateId",
                principalSchema: "public",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_NtsNote_UdfNoteId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_Template_UdfTemplateId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_NtsNote_UdfNoteId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_Template_UdfTemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_UdfNoteId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_UdfTemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_UdfNoteId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_UdfTemplateId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "UdfNoteId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "UdfNoteId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "UdfTemplateId",
                schema: "public",
                table: "NtsService");
        }
    }
}
