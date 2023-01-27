using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210211_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobId",
                schema: "rec",
                table: "Batch",
                newName: "JobAdvertisementId");

            migrationBuilder.AddColumn<string>(
                name: "HiringManager",
                schema: "rec",
                table: "Batch",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_HiringManager",
                schema: "rec",
                table: "Batch",
                column: "HiringManager");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_JobAdvertisementId",
                schema: "rec",
                table: "Batch",
                column: "JobAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStateTrack_ChangedBy",
                schema: "rec",
                table: "ApplicationStateTrack",
                column: "ChangedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationStateTrack_User_ChangedBy",
                schema: "rec",
                table: "ApplicationStateTrack",
                column: "ChangedBy",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Batch_JobAdvertisement_JobAdvertisementId",
                schema: "rec",
                table: "Batch",
                column: "JobAdvertisementId",
                principalSchema: "rec",
                principalTable: "JobAdvertisement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Batch_User_HiringManager",
                schema: "rec",
                table: "Batch",
                column: "HiringManager",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationStateTrack_User_ChangedBy",
                schema: "rec",
                table: "ApplicationStateTrack");

            migrationBuilder.DropForeignKey(
                name: "FK_Batch_JobAdvertisement_JobAdvertisementId",
                schema: "rec",
                table: "Batch");

            migrationBuilder.DropForeignKey(
                name: "FK_Batch_User_HiringManager",
                schema: "rec",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_Batch_HiringManager",
                schema: "rec",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_Batch_JobAdvertisementId",
                schema: "rec",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationStateTrack_ChangedBy",
                schema: "rec",
                table: "ApplicationStateTrack");

            migrationBuilder.DropColumn(
                name: "HiringManager",
                schema: "rec",
                table: "Batch");

            migrationBuilder.RenameColumn(
                name: "JobAdvertisementId",
                schema: "rec",
                table: "Batch",
                newName: "JobId");
        }
    }
}
