using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210310_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageId",
                schema: "public",
                table: "Permission",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PageId",
                schema: "public",
                table: "Permission",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Page_PageId",
                schema: "public",
                table: "Permission",
                column: "PageId",
                principalSchema: "public",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Page_PageId",
                schema: "public",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_PageId",
                schema: "public",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "PageId",
                schema: "public",
                table: "Permission");
        }
    }
}
