using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210210_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRoleId",
                schema: "rec",
                table: "ManpowerSummaryComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerSummaryComment_UserRoleId",
                schema: "rec",
                table: "ManpowerSummaryComment",
                column: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManpowerSummaryComment_UserRole_UserRoleId",
                schema: "rec",
                table: "ManpowerSummaryComment",
                column: "UserRoleId",
                principalSchema: "public",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManpowerSummaryComment_UserRole_UserRoleId",
                schema: "rec",
                table: "ManpowerSummaryComment");

            migrationBuilder.DropIndex(
                name: "IX_ManpowerSummaryComment_UserRoleId",
                schema: "rec",
                table: "ManpowerSummaryComment");

            migrationBuilder.DropColumn(
                name: "UserRoleId",
                schema: "rec",
                table: "ManpowerSummaryComment");
        }
    }
}
