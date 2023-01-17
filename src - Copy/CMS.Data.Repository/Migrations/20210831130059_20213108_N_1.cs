using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20213108_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentId",
                schema: "log",
                table: "NtsTaskCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentId",
                schema: "public",
                table: "NtsTaskComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentId",
                schema: "log",
                table: "NtsServiceCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentId",
                schema: "public",
                table: "NtsServiceComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentId",
                schema: "log",
                table: "NtsNoteCommentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentId",
                schema: "public",
                table: "NtsNoteComment",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentId",
                schema: "log",
                table: "NtsTaskCommentLog");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                schema: "public",
                table: "NtsTaskComment");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                schema: "log",
                table: "NtsServiceCommentLog");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                schema: "public",
                table: "NtsServiceComment");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                schema: "log",
                table: "NtsNoteCommentLog");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                schema: "public",
                table: "NtsNoteComment");
        }
    }
}
