using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210428_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToType",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToTypeId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_AssignedToTypeId",
                schema: "public",
                table: "StepTaskComponent",
                column: "AssignedToTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_LOV_AssignedToTypeId",
                schema: "public",
                table: "StepTaskComponent",
                column: "AssignedToTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_LOV_AssignedToTypeId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_AssignedToTypeId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "AssignedToTypeId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToType",
                schema: "public",
                table: "StepTaskComponent",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
