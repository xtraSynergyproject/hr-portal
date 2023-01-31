using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210319_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.RenameColumn(
                name: "TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask",
                newName: "TaskActionId");

            migrationBuilder.RenameIndex(
                name: "IX_NtsTask_TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask",
                newName: "IX_NtsTask_TaskActionId");

            migrationBuilder.AddColumn<string>(
                name: "AssignToTypeId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_NtsTask_AssignToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "AssignToTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_AssignToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "AssignToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_TaskActionId",
                schema: "public",
                table: "NtsTask",
                column: "TaskActionId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_AssignToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_TaskActionId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropIndex(
                name: "IX_NtsTask_AssignToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "AssignToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.RenameColumn(
                name: "TaskActionId",
                schema: "public",
                table: "NtsTask",
                newName: "TaskAssignToTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_NtsTask_TaskActionId",
                schema: "public",
                table: "NtsTask",
                newName: "IX_NtsTask_TaskAssignToTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_TaskAssignToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "TaskAssignToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
