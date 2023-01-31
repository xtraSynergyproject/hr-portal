using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210326_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PriorityId",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplate_PriorityId",
                schema: "public",
                table: "TaskTemplate",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplate_LOV_PriorityId",
                schema: "public",
                table: "TaskTemplate",
                column: "PriorityId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplate_LOV_PriorityId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplate_PriorityId",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                schema: "public",
                table: "TaskTemplate");
        }
    }
}
