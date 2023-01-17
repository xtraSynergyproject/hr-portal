using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210417_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableTimeEntry",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "public",
                table: "NtsTaskTimeEntry",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskTimeEntry_UserId",
                schema: "public",
                table: "NtsTaskTimeEntry",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskTimeEntry_User_UserId",
                schema: "public",
                table: "NtsTaskTimeEntry",
                column: "UserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskTimeEntry_User_UserId",
                schema: "public",
                table: "NtsTaskTimeEntry");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskTimeEntry_UserId",
                schema: "public",
                table: "NtsTaskTimeEntry");

            migrationBuilder.DropColumn(
                name: "EnableTimeEntry",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "NtsTaskTimeEntry");
        }
    }
}
