using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210406_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceActionId",
                schema: "public",
                table: "NtsService",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsService_ServiceActionId",
                schema: "public",
                table: "NtsService",
                column: "ServiceActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsService_LOV_ServiceActionId",
                schema: "public",
                table: "NtsService",
                column: "ServiceActionId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsService_LOV_ServiceActionId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropIndex(
                name: "IX_NtsService_ServiceActionId",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "ServiceActionId",
                schema: "public",
                table: "NtsService");
        }
    }
}
