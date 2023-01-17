using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210403_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskShared_NtsTask_NtsTaskId",
                schema: "public",
                table: "NtsTaskShared");

            migrationBuilder.DropIndex(
                name: "IX_NtsTaskShared_NtsTaskId",
                schema: "public",
                table: "NtsTaskShared");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NtsTaskShared_NtsTaskId",
                schema: "public",
                table: "NtsTaskShared",
                column: "NtsTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskShared_NtsTask_NtsTaskId",
                schema: "public",
                table: "NtsTaskShared",
                column: "NtsTaskId",
                principalSchema: "public",
                principalTable: "NtsTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
