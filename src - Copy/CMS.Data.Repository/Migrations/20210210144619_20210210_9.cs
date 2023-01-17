using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210210_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobAdvertisementId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Application_JobAdvertisementId",
                schema: "rec",
                table: "Application",
                column: "JobAdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_JobAdvertisement_JobAdvertisementId",
                schema: "rec",
                table: "Application",
                column: "JobAdvertisementId",
                principalSchema: "rec",
                principalTable: "JobAdvertisement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_JobAdvertisement_JobAdvertisementId",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropIndex(
                name: "IX_Application_JobAdvertisementId",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "JobAdvertisementId",
                schema: "rec",
                table: "Application");
        }
    }
}
