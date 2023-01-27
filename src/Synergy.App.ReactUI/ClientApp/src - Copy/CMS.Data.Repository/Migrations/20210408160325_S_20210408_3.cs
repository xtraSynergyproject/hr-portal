using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210408_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                schema: "public",
                table: "NtsService",
                newName: "ServiceSubject");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "public",
                table: "NtsService",
                newName: "ServiceDescription");

            migrationBuilder.RenameColumn(
                name: "Subject",
                schema: "public",
                table: "NtsNote",
                newName: "NoteSubject");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "public",
                table: "NtsNote",
                newName: "NoteDescription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceSubject",
                schema: "public",
                table: "NtsService",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "ServiceDescription",
                schema: "public",
                table: "NtsService",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NoteSubject",
                schema: "public",
                table: "NtsNote",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "NoteDescription",
                schema: "public",
                table: "NtsNote",
                newName: "Description");
        }
    }
}
