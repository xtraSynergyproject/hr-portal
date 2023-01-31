using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210306_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batch_JobAdvertisement_JobAdvertisementId",
                schema: "rec",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_Batch_JobAdvertisementId",
                schema: "rec",
                table: "Batch");

            migrationBuilder.RenameColumn(
                name: "JobAdvertisementId",
                schema: "rec",
                table: "Batch",
                newName: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobId",
                schema: "rec",
                table: "Batch",
                newName: "JobAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_JobAdvertisementId",
                schema: "rec",
                table: "Batch",
                column: "JobAdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batch_JobAdvertisement_JobAdvertisementId",
                schema: "rec",
                table: "Batch",
                column: "JobAdvertisementId",
                principalSchema: "rec",
                principalTable: "JobAdvertisement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
