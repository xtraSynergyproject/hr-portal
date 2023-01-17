using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210225_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiringMaster_User_UserId",
                schema: "rec",
                table: "HiringMaster");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HiringMaster",
                schema: "rec",
                table: "HiringMaster");

            migrationBuilder.RenameTable(
                name: "HiringMaster",
                schema: "rec",
                newName: "HiringManager",
                newSchema: "rec");

            migrationBuilder.RenameIndex(
                name: "IX_HiringMaster_UserId",
                schema: "rec",
                table: "HiringManager",
                newName: "IX_HiringManager_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HiringManager",
                schema: "rec",
                table: "HiringManager",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HiringManager_User_UserId",
                schema: "rec",
                table: "HiringManager",
                column: "UserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiringManager_User_UserId",
                schema: "rec",
                table: "HiringManager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HiringManager",
                schema: "rec",
                table: "HiringManager");

            migrationBuilder.RenameTable(
                name: "HiringManager",
                schema: "rec",
                newName: "HiringMaster",
                newSchema: "rec");

            migrationBuilder.RenameIndex(
                name: "IX_HiringManager_UserId",
                schema: "rec",
                table: "HiringMaster",
                newName: "IX_HiringMaster_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HiringMaster",
                schema: "rec",
                table: "HiringMaster",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HiringMaster_User_UserId",
                schema: "rec",
                table: "HiringMaster",
                column: "UserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
