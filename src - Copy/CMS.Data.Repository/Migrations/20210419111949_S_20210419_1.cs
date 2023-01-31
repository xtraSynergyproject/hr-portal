using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210419_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskPrecedence_NtsTask_TaskId",
                schema: "public",
                table: "NtsTaskPrecedence");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                schema: "public",
                table: "NtsTaskPrecedence",
                newName: "NtsTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_NtsTaskPrecedence_TaskId",
                schema: "public",
                table: "NtsTaskPrecedence",
                newName: "IX_NtsTaskPrecedence_NtsTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskPrecedence_NtsTask_NtsTaskId",
                schema: "public",
                table: "NtsTaskPrecedence",
                column: "NtsTaskId",
                principalSchema: "public",
                principalTable: "NtsTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskPrecedence_NtsTask_NtsTaskId",
                schema: "public",
                table: "NtsTaskPrecedence");

            migrationBuilder.RenameColumn(
                name: "NtsTaskId",
                schema: "public",
                table: "NtsTaskPrecedence",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_NtsTaskPrecedence_NtsTaskId",
                schema: "public",
                table: "NtsTaskPrecedence",
                newName: "IX_NtsTaskPrecedence_TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskPrecedence_NtsTask_TaskId",
                schema: "public",
                table: "NtsTaskPrecedence",
                column: "TaskId",
                principalSchema: "public",
                principalTable: "NtsTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
