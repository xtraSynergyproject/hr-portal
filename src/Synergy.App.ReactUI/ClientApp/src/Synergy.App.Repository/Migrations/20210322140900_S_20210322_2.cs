using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210322_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_AssignToTypeId",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.RenameColumn(
                name: "AssignToTypeId",
                schema: "public",
                table: "NtsTask",
                newName: "TaskOwnerTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_NtsTask_AssignToTypeId",
                schema: "public",
                table: "NtsTask",
                newName: "IX_NtsTask_TaskOwnerTypeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_TaskOwnerTypeId",
                schema: "public",
                table: "NtsTask",
                column: "TaskOwnerTypeId",
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

            migrationBuilder.DropForeignKey(
                name: "FK_NtsTask_LOV_TaskOwnerTypeId",
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

            migrationBuilder.RenameColumn(
                name: "TaskOwnerTypeId",
                schema: "public",
                table: "NtsTask",
                newName: "AssignToTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_NtsTask_TaskOwnerTypeId",
                schema: "public",
                table: "NtsTask",
                newName: "IX_NtsTask_AssignToTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTask_LOV_AssignToTypeId",
                schema: "public",
                table: "NtsTask",
                column: "AssignToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
