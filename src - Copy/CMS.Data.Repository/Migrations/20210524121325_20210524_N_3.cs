using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210524_N_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_UserSet_UserSetId",
                schema: "public",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserSetLog_UserSetLogId",
                schema: "public",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserSetId",
                schema: "public",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserSetLogId",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserSetId",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserSetLogId",
                schema: "public",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserSetId",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UserSetLogId",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserSetId",
                schema: "public",
                table: "User",
                column: "UserSetId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserSetLogId",
                schema: "public",
                table: "User",
                column: "UserSetLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserSet_UserSetId",
                schema: "public",
                table: "User",
                column: "UserSetId",
                principalSchema: "public",
                principalTable: "UserSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserSetLog_UserSetLogId",
                schema: "public",
                table: "User",
                column: "UserSetLogId",
                principalSchema: "log",
                principalTable: "UserSetLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
