using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210217T1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskStatus",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceTypeCode",
                schema: "public",
                table: "File",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTypeId",
                schema: "public",
                table: "File",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_TaskStatus",
                schema: "public",
                table: "NtsTask",
                column: "TaskStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_ListOfValue_TaskStatus",
                schema: "public",
                table: "NtsTask",
                column: "TaskStatus",
                principalSchema: "rec",
                principalTable: "ListOfValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_ListOfValue_TaskStatus",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_TaskStatus",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeCode",
                schema: "public",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeId",
                schema: "public",
                table: "File");
        }
    }
}
