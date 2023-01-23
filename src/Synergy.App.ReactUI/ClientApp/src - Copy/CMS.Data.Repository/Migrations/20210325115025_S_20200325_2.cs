using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20200325_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToType",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToTypeId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_AssignedToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "AssignedToTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_AssignedToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "AssignedToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_AssignedToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_AssignedToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "AssignedToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToType",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
