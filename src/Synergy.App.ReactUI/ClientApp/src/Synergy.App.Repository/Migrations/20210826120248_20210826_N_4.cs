using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210826_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsNoteComment_User_CommentToUserId",
                schema: "public",
                table: "NtsNoteComment");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsNoteCommentLog_User_CommentToUserId",
                schema: "log",
                table: "NtsNoteCommentLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsServiceComment_User_CommentToUserId",
                schema: "public",
                table: "NtsServiceComment");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsServiceCommentLog_User_CommentToUserId",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskComment_User_CommentToUserId",
                schema: "public",
                table: "NtsTaskComment");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskCommentLog_User_CommentToUserId",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskCommentLog_CommentToUserId",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskComment_CommentToUserId",
                schema: "public",
                table: "NtsTaskComment");

            migrationBuilder.DropIndex(
                name: "IX_NtsServiceCommentLog_CommentToUserId",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsServiceComment_CommentToUserId",
                schema: "public",
                table: "NtsServiceComment");

            migrationBuilder.DropIndex(
                name: "IX_NtsNoteCommentLog_CommentToUserId",
                schema: "log",
                table: "NtsNoteCommentLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsNoteComment_CommentToUserId",
                schema: "public",
                table: "NtsNoteComment");

            migrationBuilder.DropColumn(
                name: "CommentToUserId",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropColumn(
                name: "CommentToUserId",
                schema: "public",
                table: "NtsTaskComment");

            migrationBuilder.DropColumn(
                name: "CommentToUserId",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropColumn(
                name: "CommentToUserId",
                schema: "public",
                table: "NtsServiceComment");

            migrationBuilder.DropColumn(
                name: "CommentToUserId",
                schema: "log",
                table: "NtsNoteCommentLog");

            migrationBuilder.DropColumn(
                name: "CommentToUserId",
                schema: "public",
                table: "NtsNoteComment");

            migrationBuilder.AddColumn<int>(
                name: "CommentedTo",
                schema: "log",
                table: "NtsTaskCommentLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentedTo",
                schema: "public",
                table: "NtsTaskComment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentedTo",
                schema: "log",
                table: "NtsServiceCommentLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentedTo",
                schema: "public",
                table: "NtsServiceComment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentedTo",
                schema: "log",
                table: "NtsNoteCommentLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentedTo",
                schema: "public",
                table: "NtsNoteComment",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentedTo",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropColumn(
                name: "CommentedTo",
                schema: "public",
                table: "NtsTaskComment");

            migrationBuilder.DropColumn(
                name: "CommentedTo",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropColumn(
                name: "CommentedTo",
                schema: "public",
                table: "NtsServiceComment");

            migrationBuilder.DropColumn(
                name: "CommentedTo",
                schema: "log",
                table: "NtsNoteCommentLog");

            migrationBuilder.DropColumn(
                name: "CommentedTo",
                schema: "public",
                table: "NtsNoteComment");

            migrationBuilder.AddColumn<string>(
                name: "CommentToUserId",
                schema: "log",
                table: "NtsTaskCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentToUserId",
                schema: "public",
                table: "NtsTaskComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentToUserId",
                schema: "log",
                table: "NtsServiceCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentToUserId",
                schema: "public",
                table: "NtsServiceComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentToUserId",
                schema: "log",
                table: "NtsNoteCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CommentToUserId",
                schema: "public",
                table: "NtsNoteComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskCommentLog_CommentToUserId",
                schema: "log",
                table: "NtsTaskCommentLog",
                column: "CommentToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskComment_CommentToUserId",
                schema: "public",
                table: "NtsTaskComment",
                column: "CommentToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceCommentLog_CommentToUserId",
                schema: "log",
                table: "NtsServiceCommentLog",
                column: "CommentToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceComment_CommentToUserId",
                schema: "public",
                table: "NtsServiceComment",
                column: "CommentToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteCommentLog_CommentToUserId",
                schema: "log",
                table: "NtsNoteCommentLog",
                column: "CommentToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteComment_CommentToUserId",
                schema: "public",
                table: "NtsNoteComment",
                column: "CommentToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNoteComment_User_CommentToUserId",
                schema: "public",
                table: "NtsNoteComment",
                column: "CommentToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNoteCommentLog_User_CommentToUserId",
                schema: "log",
                table: "NtsNoteCommentLog",
                column: "CommentToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsServiceComment_User_CommentToUserId",
                schema: "public",
                table: "NtsServiceComment",
                column: "CommentToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsServiceCommentLog_User_CommentToUserId",
                schema: "log",
                table: "NtsServiceCommentLog",
                column: "CommentToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskComment_User_CommentToUserId",
                schema: "public",
                table: "NtsTaskComment",
                column: "CommentToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskCommentLog_User_CommentToUserId",
                schema: "log",
                table: "NtsTaskCommentLog",
                column: "CommentToUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
