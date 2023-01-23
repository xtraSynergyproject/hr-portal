using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210310_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoteActionStatusId",
                schema: "public",
                table: "NtsNote",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
          

            migrationBuilder.CreateIndex(
                name: "IX_NtsNote_NoteActionStatusId",
                schema: "public",
                table: "NtsNote",
                column: "NoteActionStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsNote_LOV_NoteActionStatusId",
                schema: "public",
                table: "NtsNote",
                column: "NoteActionStatusId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsNote_LOV_NoteActionStatusId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropIndex(
                name: "IX_NtsNote_NoteActionStatusId",
                schema: "public",
                table: "NtsNote");

            migrationBuilder.DropColumn(
                name: "NoteActionStatusId",
                schema: "public",
                table: "NtsNote");
 
        }
    }
}
