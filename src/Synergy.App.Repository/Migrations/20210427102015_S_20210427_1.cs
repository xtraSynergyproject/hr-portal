using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210427_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionId",
                schema: "public",
                table: "ProcessDesign",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "BusinessLogicExecutionType",
                schema: "public",
                table: "ProcessDesign",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProcessDesignType",
                schema: "public",
                table: "ProcessDesign",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDesign_ActionId",
                schema: "public",
                table: "ProcessDesign",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDesign_LOV_ActionId",
                schema: "public",
                table: "ProcessDesign",
                column: "ActionId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDesign_LOV_ActionId",
                schema: "public",
                table: "ProcessDesign");

            migrationBuilder.DropIndex(
                name: "IX_ProcessDesign_ActionId",
                schema: "public",
                table: "ProcessDesign");

            migrationBuilder.DropColumn(
                name: "ActionId",
                schema: "public",
                table: "ProcessDesign");

            migrationBuilder.DropColumn(
                name: "BusinessLogicExecutionType",
                schema: "public",
                table: "ProcessDesign");

            migrationBuilder.DropColumn(
                name: "ProcessDesignType",
                schema: "public",
                table: "ProcessDesign");
        }
    }
}
