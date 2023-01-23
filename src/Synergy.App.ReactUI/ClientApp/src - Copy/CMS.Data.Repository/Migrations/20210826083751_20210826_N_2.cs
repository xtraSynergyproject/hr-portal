using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210826_N_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskNo",
                schema: "public",
                table: "NtsTaskEvent",
                newName: "TaskId");

            migrationBuilder.RenameColumn(
                name: "TaskNo",
                schema: "public",
                table: "NtsServiceEvent",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "NoteNo",
                schema: "public",
                table: "NtsNoteEvent",
                newName: "NoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskId",
                schema: "public",
                table: "NtsTaskEvent",
                newName: "TaskNo");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                schema: "public",
                table: "NtsServiceEvent",
                newName: "TaskNo");

            migrationBuilder.RenameColumn(
                name: "NoteId",
                schema: "public",
                table: "NtsNoteEvent",
                newName: "NoteNo");
        }
    }
}
