using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210315_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PortalId",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_MenuGroup_PortalId",
                schema: "public",
                table: "MenuGroup",
                column: "PortalId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuGroup_Portal_PortalId",
                schema: "public",
                table: "MenuGroup",
                column: "PortalId",
                principalSchema: "public",
                principalTable: "Portal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuGroup_Portal_PortalId",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.DropIndex(
                name: "IX_MenuGroup_PortalId",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.DropColumn(
                name: "PortalId",
                schema: "public",
                table: "MenuGroup");
        }
    }
}
