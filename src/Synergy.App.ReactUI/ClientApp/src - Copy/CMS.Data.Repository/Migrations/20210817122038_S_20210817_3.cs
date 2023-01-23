using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210817_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SponsorId",
                schema: "log",
                table: "UserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SponsorId",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UserLog_SponsorId",
                schema: "log",
                table: "UserLog",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SponsorId",
                schema: "public",
                table: "User",
                column: "SponsorId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_LOV_SponsorId",
                schema: "public",
                table: "User",
                column: "SponsorId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLog_LOV_SponsorId",
                schema: "log",
                table: "UserLog",
                column: "SponsorId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_LOV_SponsorId",
                schema: "public",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLog_LOV_SponsorId",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropIndex(
                name: "IX_UserLog_SponsorId",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropIndex(
                name: "IX_User_SponsorId",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SponsorId",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "SponsorId",
                schema: "public",
                table: "User");
        }
    }
}
