using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210704_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsNoteShared_User_SharedWithTeamId",
                schema: "public",
                table: "NtsNoteShared");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsNoteSharedLog_User_SharedWithTeamId",
                schema: "log",
                table: "NtsNoteSharedLog");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNoteShared_Team_SharedWithTeamId",
                schema: "public",
                table: "NtsNoteShared",
                column: "SharedWithTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNoteSharedLog_Team_SharedWithTeamId",
                schema: "log",
                table: "NtsNoteSharedLog",
                column: "SharedWithTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsNoteShared_Team_SharedWithTeamId",
                schema: "public",
                table: "NtsNoteShared");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsNoteSharedLog_Team_SharedWithTeamId",
                schema: "log",
                table: "NtsNoteSharedLog");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNoteShared_User_SharedWithTeamId",
                schema: "public",
                table: "NtsNoteShared",
                column: "SharedWithTeamId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNoteSharedLog_User_SharedWithTeamId",
                schema: "log",
                table: "NtsNoteSharedLog",
                column: "SharedWithTeamId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
