using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210224_N_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                schema: "rec",
                table: "ListOfValue",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfValue_ParentId",
                schema: "rec",
                table: "ListOfValue",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListOfValue_ListOfValue_ParentId",
                schema: "rec",
                table: "ListOfValue",
                column: "ParentId",
                principalSchema: "rec",
                principalTable: "ListOfValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListOfValue_ListOfValue_ParentId",
                schema: "rec",
                table: "ListOfValue");

            migrationBuilder.DropIndex(
                name: "IX_ListOfValue_ParentId",
                schema: "rec",
                table: "ListOfValue");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "rec",
                table: "ListOfValue");
        }
    }
}
